using Confluent.Kafka;
using InventoryService.DataBase;
using InventoryService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Kafka;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaConsumerService> _logger;


    public KafkaConsumerService(ILogger<KafkaConsumerService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = "inventory-service",
            BootstrapServers = _configuration["Kafka:Broker"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        Task.Run(async () =>
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("sales-created");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

                    var result = consumer.Consume(stoppingToken);
                    var sale = JsonSerializer.Deserialize<Sale>(result.Message.Value);
                    if (sale != null)
                    {

                        try
                        {
                            var requestDataBase = dbContext.InventoryItem.FirstOrDefault(item => item.ProductExternalId == sale.ProductExternalId);

                            if (requestDataBase != null)
                            {
                                if (requestDataBase.QuantityAvailable > 0)
                                {
                                    var newQuantityAvailable = requestDataBase.QuantityAvailable - sale.Quantity;
                                    
                                    if (newQuantityAvailable > 0 )
                                    {
                                        requestDataBase.QuantityAvailable = newQuantityAvailable;
                                        await dbContext.SaveChangesAsync();
                                    }

                                    //_logger.LogWarning($"Não ha itens suficiente para atulizar");
                                }
                            }

                            //_logger.LogWarning($"Item não encontrado no estoque");
                            _logger.LogInformation($"Estoque atulizado com sucesso");
                        }
                        catch (Exception ex) 
                        {
                            _logger.LogInformation($"Erro ao atulizado o estoque {ex.Message}");
                        }

                        _logger.LogError($"[Inventory] Processing sale {sale.Id}: Product {sale.Product}, Qty {sale.Quantity}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        });

        return Task.CompletedTask;
    }

    //public InventoryItem VerifyItensOnDataBase(InventoryItem inventoryItem)
    //{
    //    if (inventoryItem == null)
    //    {
    //        return;
    //    }

    //    return inventoryItem;
    //}
}

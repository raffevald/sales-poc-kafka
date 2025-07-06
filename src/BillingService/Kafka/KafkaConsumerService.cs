using Confluent.Kafka;
using BillingService.Models;
using BillingService.Data;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BillingService.Kafka;

public class KafkaConsumerService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(ILogger<KafkaConsumerService> logger, IServiceProvider provider, IConfiguration configuration)
    {
        _logger = logger;
        _provider = provider;
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = "billing-service",
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
                    var result = consumer.Consume(stoppingToken);
                    var sale = JsonSerializer.Deserialize<Sale>(result.Message.Value);

                    if (sale != null)
                    {
                        using var scope = _provider.CreateScope();
                        var db = scope.ServiceProvider.GetRequiredService<BillingDbContext>();

                        var invoice = new Invoice
                        {
                            SaleExternalId = sale.Id,
                            Product = sale.Product,
                            Amount = sale.TotalPrice
                        };

                        db.Invoices.Add(invoice);
                        await db.SaveChangesAsync(stoppingToken);

                        _logger.LogInformation($"[Billing] Creating invoice for sale {sale.Id}");
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
}

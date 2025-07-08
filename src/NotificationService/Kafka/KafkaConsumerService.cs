using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Models;
using NotificationService.Services;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Kafka;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly WebhookDispatcher _webhookDispatcher;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(ILogger<KafkaConsumerService> logger, WebhookDispatcher webhookDispatcher, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _webhookDispatcher = webhookDispatcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = ConsumeKafkaAndSendWebhookAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ConsumeKafkaAndSendWebhookAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration["Kafka:Broker"],
            GroupId = "notification-service",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

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
                    var payload = MapWebhookPayload(sale);
                    var webhookUrl = "https://webhook.site/b893227f-dc75-423a-9c96-05c86de792f0";

                    await _webhookDispatcher.SendWebhookAsync(webhookUrl, payload, stoppingToken);

                    _logger.LogInformation("[Notification] Notifying customer about sale {Id} of {Product} - R${Price:F2}",
                        sale.Id, sale.Product, sale.TotalPrice);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka consumer stopped.");
            consumer.Close();
        }
    }

    public WebhookPayload MapWebhookPayload(Sale request)
    {
        var payload = new WebhookPayload
        {
            Event = "sale.created",
            Data = request
        };

        return payload;
    }
}

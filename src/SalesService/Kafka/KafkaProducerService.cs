using Confluent.Kafka;
using System.Text.Json;
using SalesService.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SalesService.Data;

namespace SalesService.Kafka;

public class KafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private const string Topic = "sales-created";

    public KafkaProducerService(IConfiguration configuration)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:Broker"]
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishSaleAsync(Sale sale)
    {
        var message = new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(sale)
        };

        await _producer.ProduceAsync(Topic, message);
    }
}

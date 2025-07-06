using Confluent.Kafka;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SalesService.Data;
using SalesService.Kafka;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<KafkaProducerService>();

var configuration = builder.Configuration;

// Substitua pelas suas strings de conexão reais:
var postgresConnection = configuration.GetConnectionString("DefaultConnection");
var kafkaBroker = configuration["Kafka:Broker"];
var kafkaBrokerConfig = new ProducerConfig
{
    BootstrapServers = kafkaBroker
};

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("API", () => HealthCheckResult.Healthy("API OK"))
    .AddNpgSql(postgresConnection, name: "PostgreSQL")
    .AddKafka(kafkaBrokerConfig, name: "Kafka");

builder.Services.AddHealthChecksUI(options =>
{
    options.AddHealthCheckEndpoint("Status", "/healthz");
}).AddInMemoryStorage();

var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(); // /healthchecks-ui


app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Kafka;
using NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddHttpClient<WebhookDispatcher>();

var app = builder.Build();

app.Run();

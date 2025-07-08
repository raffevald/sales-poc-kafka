using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace NotificationService.Services
{
    public class WebhookDispatcher
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WebhookDispatcher> _logger;

        public WebhookDispatcher(HttpClient httpClient, ILogger<WebhookDispatcher> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendWebhookAsync(string url, object payload, CancellationToken cancellationToken = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Webhook POST failed: {StatusCode}", response.StatusCode);
                }
                else
                {
                    _logger.LogInformation("Webhook sent successfully to {Url}", url);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while sending webhook to {Url}", url);
            }
        }
    }
}

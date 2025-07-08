using System;

namespace NotificationService.Models
{
    public class WebhookPayload
    {
        public string Event { get; set; } = string.Empty;
        public object Data { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

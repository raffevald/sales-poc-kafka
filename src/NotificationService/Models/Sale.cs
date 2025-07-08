using System;

namespace NotificationService.Models;

public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }

    public Guid ProductExternalId { get; set; }

    public DateTime? DeleteAt { get; set; } = null;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

using System;

namespace BillingService.Models;

public class Invoice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SaleExternalId { get; set; }
    public string Product { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeleteAt { get; set; } = null;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

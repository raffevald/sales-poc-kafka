using System;

namespace InventoryService.Models;

public class InventoryItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Product { get; set; } = string.Empty;
    public int QuantityAvailable { get; set; }

    public Guid ProductExternalId { get; set; }

    public DateTime? DeleteAt { get; set; } = null;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

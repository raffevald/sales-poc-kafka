using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace NotificationService.Models;

[Table("client_webhooks")]
public class ClientWebhook
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("client_id")]
    public Guid ClientId { get; set; }

    [Column("url")]
    [Required]
    public string Url { get; set; } = string.Empty;

    [Column("active")]
    public bool Active { get; set; } = true;

    [Column("event_type")]
    [Required]
    public string EventType { get; set; } = string.Empty;

    [Column("delete_at")]
    public DateTimeOffset? DeleteAt { get; set; }

    [Column("update_at")]
    public DateTimeOffset UpdateAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

using InventoryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.DataBase.EntitiesConfiguration;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> entity)
    {
        entity.ToTable("inventory_items");
        entity.HasKey(e => e.Id).HasName("pk_inventory_items");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Product).HasColumnName("product");
        entity.Property(e => e.QuantityAvailable).HasColumnName("quantity_available");

        entity.Property(e => e.ProductExternalId).HasColumnName("product_external_id");

        entity.Property(e => e.DeleteAt).HasColumnType("date").HasColumnName("delete_at");
        entity.Property(e => e.UpdateAt).HasColumnType("date").HasColumnName("update_at");
        entity.Property(e => e.CreatedAt).HasColumnType("date").HasColumnName("created_at");
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesService.Models;

namespace SalesService.DataBase.EntitiesConfiguration;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> entity)
    {
        entity.ToTable("orders");
        entity.HasKey(e => e.Id).HasName("pk_orders");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Product).HasColumnName("product");
        entity.Property(e => e.Quantity).HasColumnName("quantity");
        entity.Property(e => e.TotalPrice).HasColumnName("total_price");

        entity.Property(e => e.ProductExternalId).HasColumnName("product_external_id");

        entity.Property(e => e.DeleteAt).HasColumnType("date").HasColumnName("delete_at");
        entity.Property(e => e.UpdateAt).HasColumnType("date").HasColumnName("update_at");
        entity.Property(e => e.CreatedAt).HasColumnType("date").HasColumnName("created_at");
    }
}

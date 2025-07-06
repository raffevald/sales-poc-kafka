using BillingService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingService.DataBase.EntitiesConfiguration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> entity)
        {
            entity.ToTable("invoices");
            entity.HasKey(e => e.Id).HasName("pk_invoices");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.SaleExternalId).HasColumnName("sale_external_id");
            entity.Property(e => e.Product).HasColumnName("product");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.IssuedAt).HasColumnName("issued_at");

            entity.Property(e => e.DeleteAt).HasColumnType("date").HasColumnName("delete_at");
            entity.Property(e => e.UpdateAt).HasColumnType("date").HasColumnName("update_at");
            entity.Property(e => e.CreatedAt).HasColumnType("date").HasColumnName("created_at");
        }
    }
}

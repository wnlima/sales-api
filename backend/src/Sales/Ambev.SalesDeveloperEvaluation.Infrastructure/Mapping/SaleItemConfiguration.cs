using Ambev.SalesDeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.SalesDeveloperEvaluation.Infrastructure.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.HasKey(si => si.Id);

            builder.Property(si => si.Id)
                   .IsRequired();

            builder.Property(si => si.SaleId)
                   .IsRequired();

            builder.Property(si => si.ProductId)
                   .IsRequired();

            builder.Property(si => si.Quantity)
                   .IsRequired();

            builder.Property(si => si.UnitPrice)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(si => si.TotalAmount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(si => si.Discount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Ignore(si => si.DomainEvents);
        }
    }
}
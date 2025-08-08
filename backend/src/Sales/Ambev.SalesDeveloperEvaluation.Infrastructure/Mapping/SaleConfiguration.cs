using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.SalesDeveloperEvaluation.Infrastructure.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                   .IsRequired();

            builder.Property(s => s.SaleNumber)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(s => s.SaleDate)
                   .IsRequired();

            builder.Property(s => s.TotalAmount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(s => s.TotalItems)
                   .IsRequired();

            builder.Property(s => s.TotalDiscounts)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(s => s.BranchId)
                   .IsRequired();

            builder.Property(s => s.IsCancelled)
                   .IsRequired();

            builder.HasMany(s => s.Items)
                   .WithOne()
                   .HasForeignKey(item => item.SaleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Sale.Items))?
                   .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(s => s.DomainEvents);
        }
    }
}
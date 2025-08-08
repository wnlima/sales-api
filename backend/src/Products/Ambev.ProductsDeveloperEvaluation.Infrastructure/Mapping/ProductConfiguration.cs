using Ambev.ProductsDeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.ProductsDeveloperEvaluation.Infrastructure.Mapping
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            builder.Property(p => p.Price)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(p => p.QuantityInStock)
                   .IsRequired();

            builder.Ignore(p => p.DomainEvents);
        }
    }
}
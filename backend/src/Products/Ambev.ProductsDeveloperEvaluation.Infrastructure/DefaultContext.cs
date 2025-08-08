using System.Reflection;

using Ambev.ProductsDeveloperEvaluation.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Ambev.ProductsDeveloperEvaluation.Infrastructure
{
    public class DefaultContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
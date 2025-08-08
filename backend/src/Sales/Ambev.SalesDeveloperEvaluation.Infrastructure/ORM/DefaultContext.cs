using System.Reflection;

using Ambev.SalesDeveloperEvaluation.Domain.Entities;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;

using Microsoft.EntityFrameworkCore;

namespace Ambev.SalesDeveloperEvaluation.Infrastructure
{
    public class DefaultContext : DbContext
    {
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

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
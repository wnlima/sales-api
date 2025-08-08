using System.Reflection;

using Ambev.UsersDeveloperEvaluation.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Ambev.UsersDeveloperEvaluation.Infrastructure
{
    public class DefaultContext : DbContext
    {
        public DbSet<User> Users { get; set; }
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
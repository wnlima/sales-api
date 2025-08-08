using Ambev.DeveloperEvaluation.Domain.Common.Filters;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.ProductsDeveloperEvaluation.Domain.Entities;
using Ambev.ProductsDeveloperEvaluation.Domain.Repositories;
using Ambev.ProductsDeveloperEvaluation.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Ambev.ProductsDeveloperEvaluation.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        protected readonly DefaultContext _context;

        public ProductRepository(DefaultContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetQueryable(bool track)
        {
            var query = _context.Set<Product>().AsQueryable();

            if (!track)
                query = query.AsNoTracking();

            return query;
        }
        public virtual async Task<Product> CreateAsync(Product entity, CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(entity, cancellationToken);
            await this.SaveAsync(cancellationToken);
            return entity;
        }


        public virtual async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool track = false)
        {
            return await GetQueryable(track).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public virtual async Task<PaginatedList<Product>> ListAsync(AbstractAdvancedFilter filter, CancellationToken cancellationToken = default)
        {
            return await GetQueryable(false).CreatePaginatedListAsync(filter, cancellationToken);
        }

        public virtual async Task UpdateAsync(Product entity, CancellationToken cancellationToken = default)
        {
            _context.Products.Update(entity);
            await this.SaveAsync(cancellationToken);
        }

        public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken, true);

            if (entity != null)
            {
                _context.Products.Remove(entity);
                await this.SaveAsync(cancellationToken);
                return true;
            }

            return false;
        }

        public virtual async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
            _context.ChangeTracker.Clear();
        }
    }
}
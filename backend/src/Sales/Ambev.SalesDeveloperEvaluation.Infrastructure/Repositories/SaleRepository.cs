using Ambev.DeveloperEvaluation.Domain.Common.Filters;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.SalesDeveloperEvaluation.Domain.Repositories;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;
using Ambev.SalesDeveloperEvaluation.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Ambev.SalesDeveloperEvaluation.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        protected readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateAsync(Sale entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<Sale>().AddAsync(entity, cancellationToken);
            await this.SaveAsync(cancellationToken);
            return entity;
        }

        public IQueryable<Sale> GetQueryable(bool track, Guid? customerId)
        {
            var query = _context.Set<Sale>().AsQueryable();

            if (!track)
                query = query.AsNoTracking();

            if (customerId != null)
                query = ApplyUserIdFilter(query, customerId.Value);

            return query;
        }

        public async Task UpdateAsync(Sale entity, CancellationToken cancellationToken = default)
        {
            _context.Set<Sale>().Update(entity);
            await this.SaveAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, null, cancellationToken, true);

            if (entity != null)
            {
                _context.Set<Sale>().Remove(entity);
                await this.SaveAsync(cancellationToken);
                return true;
            }

            return false;
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
            _context.ChangeTracker.Clear();
        }

        public IQueryable<Sale> ApplyUserIdFilter(IQueryable<Sale> query, Guid customerId)
        {
            query = query.Where(o => o.CustomerId == customerId);
            return query;
        }

        public async Task Cancel(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Attach(sale);
            _context.Entry(sale).Property(p => p.IsCancelled).IsModified = true;
            await this.SaveAsync(cancellationToken);
        }

        public async Task<Sale?> GetByIdAsync(Guid id, Guid? customerId, CancellationToken cancellationToken = default, bool track = false)
        {
            var query = _context.Sales
                                .Where(s => s.Id == id);

            if (customerId != null)
                query = ApplyUserIdFilter(query, customerId.Value);

            if (!track)
                query = query.AsNoTracking();

            return await query
                .Include(s => s.Items)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PaginatedList<Sale>> ListAsync(AbstractAdvancedFilter filter, Guid? customerId, CancellationToken cancellationToken = default)
        {

            var query = _context.Sales.AsNoTracking().Include(s => s.Items).AsQueryable();

            if (customerId != null)
                query = ApplyUserIdFilter(query, customerId.Value);

            return await query.CreatePaginatedListAsync(filter, cancellationToken);
        }
    }
}
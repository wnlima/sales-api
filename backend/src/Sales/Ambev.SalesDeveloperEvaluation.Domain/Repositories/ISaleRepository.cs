using Ambev.DeveloperEvaluation.Domain.Common.Filters;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;

namespace Ambev.SalesDeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        IQueryable<Sale> ApplyUserIdFilter(IQueryable<Sale> query, Guid customerId);
        Task Cancel(Sale sale, CancellationToken cancellationToken = default);
        Task<Sale> CreateAsync(Sale entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Sale?> GetByIdAsync(Guid id, Guid? customerId, CancellationToken cancellationToken = default, bool track = false);
        IQueryable<Sale> GetQueryable(bool track, Guid? customerId);
        Task<PaginatedList<Sale>> ListAsync(AbstractAdvancedFilter filter, Guid? customerId, CancellationToken cancellationToken = default);
        Task SaveAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(Sale entity, CancellationToken cancellationToken = default);
    }
}
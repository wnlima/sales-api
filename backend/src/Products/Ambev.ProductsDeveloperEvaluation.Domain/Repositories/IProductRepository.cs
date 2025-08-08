using Ambev.DeveloperEvaluation.Domain.Common.Filters;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.ProductsDeveloperEvaluation.Domain.Entities;

namespace Ambev.ProductsDeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product entity, CancellationToken cancellationToken = default);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool track = false);
        Task<PaginatedList<Product>> ListAsync(AbstractAdvancedFilter filter, CancellationToken cancellationToken = default);
        Task UpdateAsync(Product entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

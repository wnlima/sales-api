
using Ambev.DeveloperEvaluation.Domain.Common.Filters;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.DTOs
{
    public class ListSalesRequest : AbstractAdvancedFilter
    {
        public Guid CustomerId { get; set; }
        public ListSalesRequest(Guid customerId, Dictionary<string, string>? filters)
        {
            Filters = filters;
            ResolveFields();
            CustomerId = customerId;
        }
    }
}

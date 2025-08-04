using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DTOs;

public class ListSalesRequest : AbstractAdvancedFilter
{
    public Guid CustomerId { get; set; }
    public ListSalesRequest(Dictionary<string, string>? filters)
    {
        Filters = filters;
        ResolveFields();
    }
}

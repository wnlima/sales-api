
using Ambev.DeveloperEvaluation.Domain.Common.Filters;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.DTOs
{
    public class ManagerListSalesRequest : AbstractAdvancedFilter
    {    public ManagerListSalesRequest(Dictionary<string, string>? filters)
        {
            Filters = filters;
            ResolveFields();
        }
    }
}

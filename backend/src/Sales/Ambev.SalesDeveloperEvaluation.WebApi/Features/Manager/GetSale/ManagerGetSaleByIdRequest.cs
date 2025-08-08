
using Ambev.DeveloperEvaluation.Domain.Common.Interfaces;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.GetSale
{
    public class ManagerGetSaleByIdRequest : IIdentifier
    {
        public Guid Id { get; set; }

        public ManagerGetSaleByIdRequest(Guid id)
        {
            Id = id;
        }
    }
}
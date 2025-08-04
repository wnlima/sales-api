using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class ManagerGetSaleByIdCommand : IRequest<GetSaleResult>
{
    public Guid Id { get; set; }
}

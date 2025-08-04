using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class ManagerCancelSaleCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
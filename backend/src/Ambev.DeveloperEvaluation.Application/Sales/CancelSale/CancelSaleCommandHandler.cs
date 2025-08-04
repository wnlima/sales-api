using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.Handlers;

public class CancelSaleCommandHandler : IRequestHandler<ManagerCancelSaleCommand, bool>
{
    private readonly ISaleService _service;
    private readonly IMapper _mapper;

    public CancelSaleCommandHandler(ISaleService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<bool> Handle(ManagerCancelSaleCommand request, CancellationToken cancellationToken)
    {
        await _service.CancelSale(request.Id, cancellationToken);
        return true;
    }
}
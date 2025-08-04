using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesCommandHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
{
    private readonly ISaleRepository _SaleRepository;
    private readonly IMapper _mapper;

    public ListSalesCommandHandler(ISaleRepository SaleRepository, IMapper mapper)
    {
        _SaleRepository = SaleRepository;
        _mapper = mapper;
    }

    public async Task<ListSalesResult> Handle(ListSalesCommand request, CancellationToken cancellationToken)
    {
        var validator = new ListSalesCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var paginatedList =
            await _SaleRepository.ListAsync(request);

        return _mapper.Map<ListSalesResult>(paginatedList);
    }
}
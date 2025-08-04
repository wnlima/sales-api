using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleByIdCommandHandler : IRequestHandler<GetSaleByIdCommand, GetSaleResult?>, IRequestHandler<ManagerGetSaleByIdCommand, GetSaleResult?>
{
    private readonly ISaleRepository _SaleRepository;
    private readonly IMapper _mapper;

    public GetSaleByIdCommandHandler(ISaleRepository SaleRepository, IMapper mapper)
    {
        _SaleRepository = SaleRepository;
        _mapper = mapper;
    }

    public async Task<GetSaleResult> Handle(GetSaleByIdCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetSaleByIdValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var Sale = await _SaleRepository.GetByIdAsync(request.Id);

        if (Sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

        return _mapper.Map<GetSaleResult>(Sale);
    }

    public async Task<GetSaleResult> Handle(ManagerGetSaleByIdCommand request, CancellationToken cancellationToken)
    {
        var validator = new ManagerGetSaleByIdValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var Sale = await _SaleRepository.GetByIdAsync(request.Id);

        if (Sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

        return _mapper.Map<GetSaleResult>(Sale);
    }
}
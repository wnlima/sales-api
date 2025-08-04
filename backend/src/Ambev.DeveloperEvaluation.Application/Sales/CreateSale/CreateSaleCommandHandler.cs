using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleService _service;
    private readonly IMapper _mapper;

    public CreateSaleCommandHandler(ISaleService SaleRepository, IMapper mapper)
    {
        _service = SaleRepository;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var Sale = _mapper.Map<Sale>(command);
        Sale = await _service.CreateSale(Sale, cancellationToken);

        return _mapper.Map<CreateSaleResult>(Sale);
    }
}
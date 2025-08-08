using Ambev.SalesDeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.GetSale
{
    public class GetSaleByIdCommandHandler : IRequestHandler<GetSaleByIdCommand, GetSaleResult?>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;

        public GetSaleByIdCommandHandler(ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetSaleResult> Handle(GetSaleByIdCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetSaleByIdValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var Sale = await _repository.GetByIdAsync(request.Id, request.CustomerId, cancellationToken);

            if (Sale == null)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

            return _mapper.Map<GetSaleResult>(Sale);
        }
    }
}
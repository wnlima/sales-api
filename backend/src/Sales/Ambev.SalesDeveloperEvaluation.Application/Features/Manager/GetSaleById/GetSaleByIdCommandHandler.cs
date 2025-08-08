using Ambev.SalesDeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Manager.GetSale
{
    public class ManagerGetSaleByIdCommandHandler : IRequestHandler<ManagerGetSaleByIdCommand, ManagerGetSaleResult?>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;

        public ManagerGetSaleByIdCommandHandler(ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ManagerGetSaleResult> Handle(ManagerGetSaleByIdCommand request, CancellationToken cancellationToken)
        {
            var validator = new ManagerGetSaleByIdValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var Sale = await _repository.GetByIdAsync(request.Id, null, cancellationToken);

            if (Sale == null)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

            return _mapper.Map<ManagerGetSaleResult>(Sale);
        }
    }
}
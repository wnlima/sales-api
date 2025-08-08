using Ambev.DeveloperEvaluation.Domain.Common.Events;
using Ambev.SalesDeveloperEvaluation.Domain.Entities;
using Ambev.SalesDeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.CreateSale
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDomainEventPublisher _publisher;

        public CreateSaleCommandHandler(ISaleRepository repository, IMapper mapper, IDomainEventPublisher publisher)
        {
            _repository = repository;
            _mapper = mapper;
            _publisher = publisher;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var items = _mapper.Map<ICollection<SaleItem>>(command.Items);

            var sale = Domain.SaleAggregate.Sale.Create(command.CustomerId, command.BranchId, items);

            await _repository.CreateAsync(sale, cancellationToken);

            await _repository.SaveAsync(cancellationToken);
            await DomainEventUtils.PublishAllDomainEventsAsync(_publisher, sale, cancellationToken);
            return _mapper.Map<CreateSaleResult>(sale);
        }
    }
}
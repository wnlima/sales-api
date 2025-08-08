using Ambev.DeveloperEvaluation.Domain.Common.Events;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.Commands;
using Ambev.SalesDeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.Handlers
{
    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDomainEventPublisher _publisher;

        public CancelSaleCommandHandler(ISaleRepository repository, IMapper mapper, IDomainEventPublisher publisher)
        {
            _repository = repository;
            _mapper = mapper;
            _publisher = publisher;
        }

        public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _repository.GetByIdAsync(request.Id, request.CustomerId, cancellationToken);

            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

            sale.Cancel();
            await _repository.UpdateAsync(sale);
            await DomainEventUtils.PublishAllDomainEventsAsync(_publisher, sale, cancellationToken);

            return true;
        }
    }
}
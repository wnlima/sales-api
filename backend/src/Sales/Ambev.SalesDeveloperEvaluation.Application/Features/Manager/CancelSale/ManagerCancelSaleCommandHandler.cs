using Ambev.DeveloperEvaluation.Domain.Common.Events;
using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.Commands;
using Ambev.SalesDeveloperEvaluation.Domain.Repositories;

using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Manager.Handlers
{
    public class ManagerCancelSaleCommandHandler : IRequestHandler<ManagerCancelSaleCommand, bool>
    {
        private readonly ISaleRepository _repository;
        private readonly IDomainEventPublisher _publisher;

        public ManagerCancelSaleCommandHandler(ISaleRepository repository, IDomainEventPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<bool> Handle(ManagerCancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _repository.GetByIdAsync(request.Id, null, cancellationToken);

            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

            sale.Cancel();
            await _repository.UpdateAsync(sale);
            await DomainEventUtils.PublishAllDomainEventsAsync(_publisher, sale, cancellationToken);

            return true;
        }
    }
}
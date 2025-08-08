using Ambev.SalesDeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.ListSales
{
    public class ListSalesCommandHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;

        public ListSalesCommandHandler(ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ListSalesResult> Handle(ListSalesCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListSalesCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var paginatedList = await _repository.ListAsync(request, request.CustomerId, cancellationToken);

            return _mapper.Map<ListSalesResult>(paginatedList);
        }
    }
}
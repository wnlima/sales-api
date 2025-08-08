using Ambev.DeveloperEvaluation.Domain.Common.Filters;
using Ambev.SalesDeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Manager.ListSales
{
    public class ManagerListSalesCommandHandler : IRequestHandler<ManagerListSalesCommand, ManagerListSalesResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;

        public ManagerListSalesCommandHandler(ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ManagerListSalesResult> Handle(ManagerListSalesCommand request, CancellationToken cancellationToken)
        {
            var validator = new ManagerListSalesCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var filter = _mapper.Map<AbstractAdvancedFilter>(request);
            var paginatedList = await _repository.ListAsync(filter, null, cancellationToken);

            return _mapper.Map<ManagerListSalesResult>(paginatedList);
        }
    }
}
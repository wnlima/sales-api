using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Validators;
using Ambev.ProductsDeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Handlers
{
    public class ListProductsCommandHandler : IRequestHandler<ListProductsCommand, ListProductsResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ListProductsCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ListProductsResult> Handle(ListProductsCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListProductsCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var paginatedList =
                await _productRepository.ListAsync(request);

            return _mapper.Map<ListProductsResult>(paginatedList);
        }
    }
}
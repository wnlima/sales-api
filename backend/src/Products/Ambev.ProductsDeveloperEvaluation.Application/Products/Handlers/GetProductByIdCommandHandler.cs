using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Validators;
using Ambev.ProductsDeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Handlers
{
    public class GetProductByIdCommandHandler : IRequestHandler<GetProductByIdCommand, GetProductResult?>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetProductResult> Handle(GetProductByIdCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetProductByIdValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");

            return _mapper.Map<GetProductResult>(product);
        }
    }
}
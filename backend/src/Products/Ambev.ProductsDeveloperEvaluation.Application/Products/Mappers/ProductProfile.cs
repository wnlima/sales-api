using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;
using Ambev.ProductsDeveloperEvaluation.Domain.Entities;

using AutoMapper;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, CreateProductResult>();
            CreateMap<Product, GetProductResult>();
            CreateMap<Product, UpdateProductResult>();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<UpdateProductCommand, Product>();
            CreateMap<PaginatedList<Product>, ListProductsResult>();
        }
    }
}

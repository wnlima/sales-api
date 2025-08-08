using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

using AutoMapper;

namespace Ambev.ProductsDeveloperEvaluation.WebApi.Mappings
{
    /// <summary>
    /// Profile for mapping between Application and API CreateProduct responses
    /// </summary>
    public class ProductProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateProduct feature
        /// </summary>
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, CreateProductCommand>();
            CreateMap<UpdateProductRequest, UpdateProductCommand>();
            CreateMap<DeleteProductRequest, DeleteProductCommand>();

            CreateMap<ListProductsRequest, ListProductsCommand>();
            CreateMap<ListProductsResult, ListProductsResponse>();
            CreateMap<CreateProductResult, CreateProductResponse>();
            CreateMap<UpdateProductResult, UpdateProductResponse>();
            CreateMap<GetProductResult, GetProductResponse>();
        }
    }
}
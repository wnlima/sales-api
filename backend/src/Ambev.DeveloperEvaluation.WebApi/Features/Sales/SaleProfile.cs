using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Profile for mapping between Application and API CreateSale responses
/// </summary>
public class SaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale feature
    /// </summary>
    public SaleProfile()
    {
        CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();
        CreateMap<ManagerCancelSaleRequest, ManagerCancelSaleCommand>();
        CreateMap<ListSalesRequest, ListSalesCommand>();
        CreateMap<SaleResult, SaleResponse>();
        CreateMap<SaleItemResult, SaleItemResponse>();
        CreateMap<GetSaleByIdRequest, GetSaleByIdCommand>();

        CreateMap<CreateSaleRequest, CreateSaleCommand>()
            .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.Items))
            .AfterMap((src, dest) =>
            {
                foreach (var item in dest.SaleItems)
                {
                    item.CustomerId = src.CustomerId;
                }
            });
    }
}
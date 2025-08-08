using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.Commands;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.CreateSale;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.GetSale;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.ListSales;
using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.Commands;
using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.GetSale;
using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.ListSales;
using Ambev.SalesDeveloperEvaluation.Application.Sales.Common;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.CancelSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.CreateSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.DTOs;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.GetSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.ListSales;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.CancelSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.DTOs;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.GetSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.ListSales;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Sales.Common;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Sales.CreateSale;

using AutoMapper;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Sales
{
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
            CreateMap<ListSalesRequest, ListSalesCommand>();
            CreateMap<CancelSaleRequest, CancelSaleCommand>();
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<GetSaleByIdRequest, GetSaleByIdCommand>();
            CreateMap<ManagerCancelSaleRequest, ManagerCancelSaleCommand>();

            CreateMap<ManagerListSalesRequest, ManagerListSalesCommand>();
            CreateMap<ManagerCancelSaleRequest, ManagerCancelSaleCommand>();
            CreateMap<ManagerGetSaleByIdRequest, ManagerGetSaleByIdCommand>();

            CreateMap<SaleResult, SaleResponse>();
            CreateMap<SaleItemResult, SaleItemResponse>();
            CreateMap<CreateSaleResult, CreateSaleResponse>();
            CreateMap<ListSalesResult, ListSalesResponse>();
            CreateMap<GetSaleResult, GetSaleResponse>();

            CreateMap<ManagerListSalesResult, ManagerListSalesResponse>();
            CreateMap<ManagerGetSaleResult, ManagerGetSaleResponse>();
        }
    }
}
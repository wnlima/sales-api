using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.CreateSale;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.GetSale;
using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.ListSales;
using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.GetSale;
using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.ListSales;
using Ambev.SalesDeveloperEvaluation.Application.Sales.Common;
using Ambev.SalesDeveloperEvaluation.Domain.Entities;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;

using AutoMapper;

namespace Ambev.SalesDeveloperEvaluation.Application.Mappers
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<SaleItem, SaleItemResult>();
            CreateMap<CreateSaleItemCommand, SaleItem>();
            CreateMap<CreateSaleCommand, Sale>();

            CreateMap<Sale, CreateSaleResult>();
            CreateMap<Sale, GetSaleResult>();
            CreateMap<Sale, SaleResult>();

            CreateMap<PaginatedList<Sale>, ListSalesResult>();

            CreateMap<Sale, ManagerGetSaleResult>();
            CreateMap<PaginatedList<Sale>, ManagerListSalesResult>();
        }
    }
}
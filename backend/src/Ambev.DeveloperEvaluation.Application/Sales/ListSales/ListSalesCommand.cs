namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesCommand : ManagerListSalesCommand
{
    public Guid CustomerId { get; set; }
}
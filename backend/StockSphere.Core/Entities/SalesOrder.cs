using StockSphere.Core.Enums;

namespace StockSphere.Core.Entities;

public class SalesOrder
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    public ICollection<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
}

using StockSphere.Core.Enums;

namespace StockSphere.Core.Entities;

public class InventoryTransaction
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    
    public TransactionType Type { get; set; }
    public int Quantity { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty; // PO number or SO number
    public string Remarks { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}

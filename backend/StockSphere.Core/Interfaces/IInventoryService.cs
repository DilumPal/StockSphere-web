namespace StockSphere.Core.Interfaces;

public interface IInventoryService
{
    Task ReceivePurchaseOrderAsync(Guid purchaseOrderId, Guid warehouseId);
    Task DispatchSalesOrderAsync(Guid salesOrderId, Guid warehouseId);
}

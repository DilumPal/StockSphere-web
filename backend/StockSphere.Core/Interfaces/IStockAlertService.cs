namespace StockSphere.Core.Interfaces;

public interface IStockAlertService
{
    Task SendLowStockAlertAsync(string productName, string sku, int currentStock, string warehouseName);
}

using Microsoft.AspNetCore.SignalR;
using StockSphere.Api.Hubs;
using StockSphere.Core.Interfaces;

namespace StockSphere.Api.Services;

public class SignalRStockAlertService : IStockAlertService
{
    private readonly IHubContext<InventoryHub> _hubContext;

    public SignalRStockAlertService(IHubContext<InventoryHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendLowStockAlertAsync(string productName, string sku, int currentStock, string warehouseName)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveLowStockAlert", new {
            productName,
            sku,
            currentStock,
            warehouseName,
            timestamp = DateTime.UtcNow
        });
    }
}

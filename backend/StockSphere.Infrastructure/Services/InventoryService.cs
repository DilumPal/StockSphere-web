using Microsoft.EntityFrameworkCore;
using StockSphere.Core.Entities;
using StockSphere.Core.Enums;
using StockSphere.Core.Interfaces;
using StockSphere.Infrastructure.Data;

namespace StockSphere.Infrastructure.Services;

public class InventoryService : IInventoryService
{
    private readonly AppDbContext _context;
    private readonly IStockAlertService _alertService;

    public InventoryService(AppDbContext context, IStockAlertService alertService)
    {
        _context = context;
        _alertService = alertService;
    }

    public async Task ReceivePurchaseOrderAsync(Guid purchaseOrderId, Guid warehouseId)
    {
        var po = await _context.PurchaseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == purchaseOrderId);

        if (po == null) throw new Exception("Purchase Order not found");
        if (po.Status == OrderStatus.Completed) throw new Exception("Order is already completed");

        foreach (var item in po.Items)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.ProductId == item.ProductId && s.WarehouseId == warehouseId);

            if (stock == null)
            {
                stock = new Stock
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    WarehouseId = warehouseId,
                    Quantity = item.Quantity,
                    LastUpdated = DateTime.UtcNow
                };
                _context.Stocks.Add(stock);
            }
            else
            {
                stock.Quantity += item.Quantity;
                stock.LastUpdated = DateTime.UtcNow;
            }

            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                WarehouseId = warehouseId,
                Type = TransactionType.In,
                Quantity = item.Quantity,
                ReferenceNumber = po.OrderNumber,
                Remarks = "Received PO",
                TransactionDate = DateTime.UtcNow
            });
        }

        po.Status = OrderStatus.Completed;
        await _context.SaveChangesAsync();
    }

    public async Task DispatchSalesOrderAsync(Guid salesOrderId, Guid warehouseId)
    {
        var so = await _context.SalesOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == salesOrderId);

        if (so == null) throw new Exception("Sales Order not found");
        if (so.Status == OrderStatus.Completed) throw new Exception("Order is already completed");

        foreach (var item in so.Items)
        {
            var stock = await _context.Stocks
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.ProductId == item.ProductId && s.WarehouseId == warehouseId);

            if (stock == null || stock.Quantity < item.Quantity)
                throw new Exception($"Insufficient stock for product {item.ProductId}");

            stock.Quantity -= item.Quantity;
            stock.LastUpdated = DateTime.UtcNow;

            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                WarehouseId = warehouseId,
                Type = TransactionType.Out,
                Quantity = item.Quantity,
                ReferenceNumber = so.OrderNumber,
                Remarks = "Dispatched SO",
                TransactionDate = DateTime.UtcNow
            });

            // Trigger real-time alert if stock drops below reorder level
            if (stock.Quantity <= stock.Product.ReorderLevel)
            {
                var warehouse = await _context.Warehouses.FindAsync(warehouseId);
                await _alertService.SendLowStockAlertAsync(stock.Product.Name, stock.Product.Sku, stock.Quantity, warehouse?.Name ?? "Unknown");
            }
        }

        so.Status = OrderStatus.Completed;
        await _context.SaveChangesAsync();
    }
}

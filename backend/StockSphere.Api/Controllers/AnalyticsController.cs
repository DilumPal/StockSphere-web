using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockSphere.Core.Enums;
using StockSphere.Infrastructure.Data;

namespace StockSphere.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnalyticsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var totalProducts = await _context.Products.CountAsync();
        
        var lowStockCount = await _context.Stocks
            .Include(s => s.Product)
            .CountAsync(s => s.Quantity <= s.Product.ReorderLevel);

        var activeOrders = await _context.SalesOrders
            .CountAsync(so => so.Status == OrderStatus.Pending || so.Status == OrderStatus.Processing);
            
        var totalStockValue = await _context.Stocks
            .Include(s => s.Product)
            .SumAsync(s => s.Quantity * s.Product.PurchasePrice);

        return Ok(new
        {
            TotalProducts = totalProducts,
            LowStockAlerts = lowStockCount,
            ActiveOrders = activeOrders,
            TotalStockValue = totalStockValue
        });
    }

    [HttpGet("stock-movements")]
    public async Task<IActionResult> GetRecentMovements()
    {
        var movements = await _context.InventoryTransactions
            .Include(t => t.Product)
            .OrderByDescending(t => t.TransactionDate)
            .Take(10)
            .Select(t => new
            {
                t.Id,
                ProductName = t.Product.Name,
                Type = t.Type.ToString(),
                t.Quantity,
                t.TransactionDate
            })
            .ToListAsync();

        return Ok(movements);
    }
}

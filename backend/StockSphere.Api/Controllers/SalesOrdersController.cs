using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockSphere.Core.Entities;
using StockSphere.Core.Enums;
using StockSphere.Core.Interfaces;
using StockSphere.Infrastructure.Data;

namespace StockSphere.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesOrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IInventoryService _inventoryService;

    public SalesOrdersController(AppDbContext context, IInventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sos = await _context.SalesOrders
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();
        return Ok(sos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var so = await _context.SalesOrders
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id);
            
        if (so == null) return NotFound();
        return Ok(so);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SalesOrder so)
    {
        so.Id = Guid.NewGuid();
        so.OrderDate = DateTime.UtcNow;
        so.Status = OrderStatus.Pending;
        
        foreach (var item in so.Items)
        {
            item.Id = Guid.NewGuid();
            item.SalesOrderId = so.Id;
        }

        _context.SalesOrders.Add(so);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = so.Id }, so);
    }

    [HttpPost("{id}/dispatch")]
    public async Task<IActionResult> Dispatch(Guid id, [FromQuery] Guid warehouseId)
    {
        try
        {
            await _inventoryService.DispatchSalesOrderAsync(id, warehouseId);
            return Ok(new { message = "Sales order dispatched and inventory updated." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

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
public class PurchaseOrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IInventoryService _inventoryService;

    public PurchaseOrdersController(AppDbContext context, IInventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pos = await _context.PurchaseOrders
            .Include(p => p.Supplier)
            .Include(p => p.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();
        return Ok(pos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var po = await _context.PurchaseOrders
            .Include(p => p.Supplier)
            .Include(p => p.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (po == null) return NotFound();
        return Ok(po);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PurchaseOrder po)
    {
        po.Id = Guid.NewGuid();
        po.OrderDate = DateTime.UtcNow;
        po.Status = OrderStatus.Pending;
        
        foreach (var item in po.Items)
        {
            item.Id = Guid.NewGuid();
            item.PurchaseOrderId = po.Id;
        }

        _context.PurchaseOrders.Add(po);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = po.Id }, po);
    }

    [HttpPost("{id}/receive")]
    public async Task<IActionResult> Receive(Guid id, [FromQuery] Guid warehouseId)
    {
        try
        {
            await _inventoryService.ReceivePurchaseOrderAsync(id, warehouseId);
            return Ok(new { message = "Purchase order received and inventory updated." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

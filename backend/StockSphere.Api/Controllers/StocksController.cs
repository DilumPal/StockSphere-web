using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockSphere.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StockSphere.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public StocksController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetStocks()
    {
        var stocks = await _dbContext.Stocks
            .Include(s => s.Product)
            .Include(s => s.Warehouse)
            .Select(s => new
            {
                s.Id,
                s.Quantity,
                s.LastUpdated,
                s.Aisle,
                s.Rack,
                s.Bin,
                Product = new
                {
                    s.Product.Id,
                    s.Product.Name,
                    s.Product.Sku,
                    s.Product.Manufacturer,
                    s.Product.PartNumber,
                    s.Product.ReorderLevel
                },
                Warehouse = new
                {
                    s.Warehouse.Id,
                    s.Warehouse.Name
                }
            })
            .ToListAsync();

        return Ok(stocks);
    }
}

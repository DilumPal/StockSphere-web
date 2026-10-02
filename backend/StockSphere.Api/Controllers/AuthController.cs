using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockSphere.Core.Entities;
using StockSphere.Core.Enums;
using StockSphere.Infrastructure.Data;
using StockSphere.Infrastructure.Services;

namespace StockSphere.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(AppDbContext dbContext, IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (user == null || user.PasswordHash != request.Password) 
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        if (!user.IsActive)
        {
            return Unauthorized(new { message = "Account is disabled" });
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return Ok(new
        {
            Token = token,
            User = new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            }
        });
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedAdmin()
    {
        if (await _dbContext.Users.AnyAsync())
        {
            return BadRequest("Database already seeded.");
        }

        var admin = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@stocksphere.com",
            PasswordHash = "admin123", // For production, implement password hashing
            Role = UserRole.Admin,
            IsActive = true
        };

        _dbContext.Users.Add(admin);
        await _dbContext.SaveChangesAsync();

        return Ok("Admin user seeded successfully. Email: admin@stocksphere.com, Password: admin123");
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

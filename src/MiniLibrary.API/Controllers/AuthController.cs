using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Infrastructure.Data;
using MiniLibrary.Domain.Entities;
using MiniLibrary.Application.DTOs;
using MiniLibrary.Application.DTOs.Register;
using MiniLibrary.Application.Services;

namespace MiniLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _jwtTokenService;
    
    public AuthController(AppDbContext context, IPasswordHasher hasher, IJwtTokenService jwtTokenService)
    {
        _context = context;
        _hasher = hasher;
        _jwtTokenService = jwtTokenService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Request body is required");
        }

        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.Username))
        {
            return BadRequest("Username, email, and password are required");
        }

        var emailLower = dto.Email.ToLower().Trim();
        var exists = await _context.Users
            .AnyAsync(u => u.Email.ToLower() == emailLower);

        if (exists)
            return BadRequest("Email already exists");

        var hashedPassword = _hasher.HashPassword(dto.Password);
        
        var user = new User
        {
            Name = dto.Username,
            Email = emailLower,
            Role = dto.Role,
            Password = hashedPassword
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        Console.WriteLine($"[DEBUG] User registered successfully: {emailLower}");
        return Ok(new { message = "User registered successfully", email = emailLower });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Request body is required");
        }

        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest("Email and password are required");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

        if (user == null)
        {
            // Log for debugging (remove in production)
            Console.WriteLine($"[DEBUG] Login failed: User with email '{dto.Email}' not found");
            return Unauthorized("Invalid credentials");
        }

        // Check if password is empty or null (shouldn't happen, but safety check)
        if (string.IsNullOrEmpty(user.Password))
        {
            Console.WriteLine($"[DEBUG] Login failed: User '{dto.Email}' has no password stored");
            return Unauthorized("Invalid credentials");
        }

        var valid = _hasher.VerifyPassword(user.Password, dto.Password);
        if (!valid)
        {
            Console.WriteLine($"[DEBUG] Login failed: Password verification failed for user '{dto.Email}'");
            return Unauthorized("Invalid credentials");
        }

        var token = _jwtTokenService.GenerateToken(user);
        Console.WriteLine($"[DEBUG] Login successful for user '{dto.Email}'");

        return Ok(new TokenResponseDto { Token = token });
    }

}
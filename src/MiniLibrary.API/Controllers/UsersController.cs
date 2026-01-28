using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MiniLibrary.Infrastructure.Data;
using MiniLibrary.Domain.Entities;
using MiniLibrary.Application.DTOs;
using MiniLibrary.Application.Services;
using Domain.Interfaces;

namespace MiniLibrary.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAuditService _auditService;
    public UsersController(AppDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }
[Authorize(Roles = "Admin")]
[HttpGet]
public IActionResult GetUsers()
    {
        var users = _context.Users
            .Select(u => new { u.Id, u.Name })
            .ToList();
        return Ok(users);
    }

[HttpGet("{id}")]
public IActionResult GetUser(int id)
{
    var user = _context.Users
        .Where(u => u.Id == id)
        .Select(u => new { u.Id, u.Name })
        .FirstOrDefault();

    if (user == null)
    {
        return NotFound();
    }
    return Ok(user);
}
[HttpPost]
public IActionResult CreateUser([FromBody] CreateUserRequest request)
{
    var user = new User
    {
        Name = request.Name
    };
    _context.Users.Add(user);
    _context.SaveChanges();
    var response = new { user.Id, user.Name };
    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
}

[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public IActionResult DeleteUser(int id)
{
   var user = _context.Users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return NotFound();
    }

    user.IsDeleted = true;
    user.DeletedAt = DateTime.UtcNow;
    _context.Users.Update(user);
    _context.SaveChanges();
    return NoContent();
}
[Authorize]
[HttpPut("{id}")]
public IActionResult UpdateUser(int id, [FromBody] CreateUserRequest request)
{
var user = _context.Users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return NotFound();
    }

    var oldName = user.Name;
    user.Name = request.Name;
    _auditService.TrackChanges(user, new Dictionary<string, object> { { "Name", $"{oldName} -> {request.Name}" } });
   _context.Users.Update(user);
    _context.SaveChanges();
    var response = new { user.Id, user.Name };
    return Ok(response);
}
[Authorize]
[HttpGet("{id}/loans")]
public IActionResult GetUserLoans(int id)
{
    var userIdClaim = User.FindFirst("UserId")?.Value;
    if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
    {
        return Unauthorized("Invalid token");
    }
    var user = _context.Users.FirstOrDefault(u => u.Id == userId);
    if (user == null)
    {
        return NotFound();
    }
    var loans = _context.Loans
        .Where(l => l.UserId == userId)
        .Select(l => new { l.Id, l.BookId, l.UserId, l.LoanDate, l.ReturnDate })
        .ToList();
    return Ok(loans);
}
[HttpGet("email/{email}")]
public IActionResult GetUserByEmail(string email)
{
    var user = _context.Users
        .Where(u => u.Email.ToLower() == email.ToLower())
        .Select(u => new { u.Id, u.Name, u.Email, u.Role })
        .FirstOrDefault();

    if (user == null)
    {
        return NotFound();
    }
    return Ok(user);
}

}


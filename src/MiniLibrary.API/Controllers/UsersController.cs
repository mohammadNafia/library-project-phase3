using Microsoft.AspNetCore.Mvc;
using MiniLibrary.Infrastructure.Data;
using MiniLibrary.Domain.Entities;
using MiniLibrary.Application.DTOs;

namespace MiniLibrary.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    public UsersController(AppDbContext context)
    {
        _context = context;
    }
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

[HttpDelete("{id}")]
public IActionResult DeleteUser(int id)
{
   var user = _context.Users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return NotFound();
    }

    _context.Users.Remove(user);
    _context.SaveChanges();
    return NoContent();
}
[HttpPut("{id}")]
public IActionResult UpdateUser(int id, [FromBody] CreateUserRequest request)
{
var user = _context.Users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return NotFound();
    }

    user.Name = request.Name;
   _context.Users.Update(user);
    _context.SaveChanges();
    var response = new { user.Id, user.Name };
    return Ok(response);
}
[HttpGet("{id}/loans")]
public IActionResult GetUserLoans(int id)
{
    var user = _context.Users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return NotFound();
    }
    var loans = _context.Loans
        .Where(l => l.UserId == id)
        .Select(l => new { l.Id, l.BookId, l.UserId, l.LoanDate, l.ReturnDate })
        .ToList();
    return Ok(loans);
}

}


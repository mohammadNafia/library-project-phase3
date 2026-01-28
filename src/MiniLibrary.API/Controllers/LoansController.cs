using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MiniLibrary.Infrastructure.Data;
using MiniLibrary.Domain.Entities;
using MiniLibrary.Application.DTOs;

namespace MiniLibrary.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly AppDbContext _context;
    public LoansController(AppDbContext context)
    {
        _context = context;
    }







[Authorize]
[HttpPost]
public IActionResult CreateLoan([FromBody] CreateLoanRequest request)
{
    var userIdClaim = User.FindFirst("UserId")?.Value;
    if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
    {
        return Unauthorized("Invalid token");
    }

    var user = _context.Users.FirstOrDefault(u => u.Id == userId);
    if (user == null)
    {
        return NotFound("User not found.");
    }
    var book = _context.Books.FirstOrDefault(b => b.Id == request.BookId);
    if (book == null)
    {
        return NotFound("Book not found.");
    }
    var loan = new Loan
    {
        UserId = userId,
        BookId = request.BookId,
        LoanDate = DateTime.UtcNow,
        ReturnDate = null
    };
    _context.Loans.Add(loan);
    _context.SaveChanges();
    
    
    var response = new
    {
        loan.Id,
        loan.BookId,
        loan.UserId,
        loan.LoanDate,
        loan.ReturnDate
    };
    return CreatedAtAction(nameof(CreateLoan), new { id = loan.Id }, response);
}
}
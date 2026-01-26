using Microsoft.AspNetCore.Mvc;
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







[HttpPost]
public IActionResult CreateLoan([FromBody] CreateLoanRequest request)
{
    var user = _context.Users.FirstOrDefault(u => u.Id == request.UserId);
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
        UserId = request.UserId,
        BookId = request.BookId,
        LoanDate = request.LoanDate,
        ReturnDate = request.ReturnDate
    };
    _context.Loans.Add(loan);
    _context.SaveChanges();
    
    // Return loan without navigation properties to avoid circular reference
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
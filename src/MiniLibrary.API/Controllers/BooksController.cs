using Microsoft.AspNetCore.Mvc;
using MiniLibrary.Infrastructure.Data;
using MiniLibrary.Domain.Entities;
using MiniLibrary.Application.DTOs;

namespace MiniLibrary.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;
    public BooksController(AppDbContext context)
    {
        _context = context;
    }
[HttpGet]
public IActionResult GetBooks()
    {
        var books = _context.Books
            .Select(b => new { b.Id, b.Title, b.Genre })
            .ToList();
        return Ok(books);
    }

[HttpGet("{id}")]
public IActionResult GetBook(int id)
{
    var book = _context.Books
        .Where(b => b.Id == id)
        .Select(b => new { b.Id, b.Title, b.Genre })
        .FirstOrDefault();

    if (book == null)
    {
        return NotFound();
    }
    return Ok(book);
}
[HttpPost]
public IActionResult CreateBook([FromBody] CreateBookRequest request)
{
    var book = new Book
    {
        Title = request.Title,
        Genre = request.Genre
    };
    _context.Books.Add(book);
    _context.SaveChanges();
    var response = new { book.Id, book.Title, book.Genre };
    return CreatedAtAction(nameof(GetBook), new { id = book.Id }, response);
}

[HttpDelete("{id}")]
public IActionResult DeleteBook(int id)
{
   var book = _context.Books.FirstOrDefault(b => b.Id == id);
    if (book == null)
    {
        return NotFound();
    }

    _context.Books.Remove(book);
    _context.SaveChanges();
    return NoContent();
}
[HttpPut("{id}")]
public IActionResult UpdateBook(int id, [FromBody] CreateBookRequest request)
{
var book = _context.Books.FirstOrDefault(b => b.Id == id);
    if (book == null)
    {
        return NotFound();
    }

    book.Title = request.Title;
    book.Genre = request.Genre;
   _context.Books.Update(book);
    _context.SaveChanges();
    var response = new { book.Id, book.Title, book.Genre };
    return Ok(response);
}

}


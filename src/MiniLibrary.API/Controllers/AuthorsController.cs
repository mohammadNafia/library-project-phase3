using Microsoft.AspNetCore.Mvc;
using MiniLibrary.Infrastructure.Data;
using MiniLibrary.Domain.Entities;
using MiniLibrary.Application.DTOs;

namespace MiniLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public AuthorsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAuthors()
    {
        var authors = _context.Authors
            .Select(a => new { a.Id, a.Name })
            .ToList();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public IActionResult GetAuthor(int id)
    {
        var author = _context.Authors
            .Where(a => a.Id == id)
            .Select(a => new { a.Id, a.Name })
            .FirstOrDefault();

        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpPost]
    public IActionResult CreateAuthor([FromBody] CreateAuthorRequest request)
    {
        var author = new Author
        {
            Name = request.Name
        };
        _context.Authors.Add(author);
        _context.SaveChanges();
        var response = new { author.Id, author.Name };
        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, response);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAuthor(int id)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Id == id);
        if (author == null)
        {
            return NotFound();
        }

        _context.Authors.Remove(author);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAuthor(int id, [FromBody] CreateAuthorRequest request)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Id == id);
        if (author == null)
        {
            return NotFound();
        }

        author.Name = request.Name;
        _context.Authors.Update(author);
        _context.SaveChanges();
        var response = new { author.Id, author.Name };
        return Ok(response);
    }
}
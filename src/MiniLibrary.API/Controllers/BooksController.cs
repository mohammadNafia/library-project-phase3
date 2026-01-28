using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Infrastructure.Data;
using MiniLibrary.Domain.Entities;
using MiniLibrary.Application.DTOs;
using MiniLibrary.Application.DTOs.Books;
using Microsoft.AspNetCore.Authorization;
using MiniLibrary.Application.Common;
using MiniLibrary.Application.Services;
using Domain.Interfaces;
using System;

namespace MiniLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;
    private readonly IAuditService _auditService;
    public BooksController(AppDbContext context, ICacheService cache, IAuditService auditService)
    {
        _context = context;
        _cache = cache;
        _auditService = auditService;
    }
    
    [HttpGet]
    public IActionResult GetBooks()
    {
        var cachedBooks = _cache.Get<List<BookDto>>("books_all");
        if (cachedBooks != null)
        {
            return Ok(ApiResponse<List<BookDto>>.Ok(cachedBooks));
        }

        var books = _context.Books
            .Include(b => b.Author)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Genre = b.Genre,
                AuthorName = b.Author.Name
            })
            .ToList();

        _cache.Set("books_all", books, TimeSpan.FromSeconds(30));
        return Ok(ApiResponse<List<BookDto>>.Ok(books));
    }
    [HttpGet("paged")]
    public IActionResult GetBooksPaged(int page = 1, int size = 5)
    {
        var query = _context.Books
            .Include(b => b.Author)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Genre = b.Genre,
                AuthorName = b.Author.Name
            });

        var total = query.Count();

        var items = query
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();

        var result = new PaginatedResult<BookDto>
        {
            Items = items,
            PageNumber = page,
            PageSize = size,
            TotalCount = total
        };

        return Ok(ApiResponse<PaginatedResult<BookDto>>.Ok(result));
    }
    
    [HttpGet("{id}")]
    public IActionResult GetBook(int id)
    {
        var book = _context.Books
            .Include(b => b.Author)
            .Where(b => b.Id == id)
            .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    AuthorName = b.Author.Name
                })
            .FirstOrDefault();

        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CreateBook([FromBody] CreateBookRequest request)
{
        var book = new Book
        {
            Title = request.Title,
            Genre = request.Genre,
            AuthorId = request.AuthorId ?? 0
        };
        _context.Books.Add(book);
        _context.SaveChanges();
        _context.Entry(book).Reference(b => b.Author).Load();
        var response = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Genre = book.Genre,
            AuthorName = book.Author.Name
        };
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

        book.IsDeleted = true;
        book.DeletedAt = DateTime.UtcNow;
        _context.Books.Update(book);
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

        var oldTitle = book.Title;
        var oldGenre = book.Genre;
        book.Title = request.Title;
        book.Genre = request.Genre;
        _auditService.TrackChanges((IAuditable)book, new Dictionary<string, object> { { "Title", $"{oldTitle} -> {request.Title}" }, { "Genre", $"{oldGenre} -> {request.Genre}" } });
        _context.Books.Update(book);
        _context.SaveChanges();
        _context.Entry(book).Reference(b => b.Author).Load();
        var response = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Genre = book.Genre,
            AuthorName = book.Author.Name
        };
        return Ok(ApiResponse<BookDto>.Ok(response));
    }

}


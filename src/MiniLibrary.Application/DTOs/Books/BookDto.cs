using MiniLibrary.Domain.Entities;

namespace MiniLibrary.Application.DTOs.Books;

public class BookDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public Genre Genre { get; set; }

    public string AuthorName { get; set; } = string.Empty;
}
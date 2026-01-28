using MiniLibrary.Domain.Entities;

namespace MiniLibrary.Application.DTOs;

public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public Genre Genre { get; set; }
    public int? AuthorId { get; set; }
}

namespace MiniLibrary.Application.DTOs;

public class CreateAuthorRequest
{
    public string Name { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Biography { get; set; } = string.Empty;
}

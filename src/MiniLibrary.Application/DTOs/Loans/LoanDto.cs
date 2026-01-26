namespace MiniLibrary.Application.DTOs;

public class LoanDto
{
public int Id { get; set; }
public string BookTitle { get; set; } = string.Empty;
public DateTime LoanDate { get; set; }
public DateTime? ReturnDate { get; set; }
}
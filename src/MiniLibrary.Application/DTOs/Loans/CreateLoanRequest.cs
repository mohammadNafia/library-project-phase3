namespace MiniLibrary.Application.DTOs;

public class CreateLoanRequest
{
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}

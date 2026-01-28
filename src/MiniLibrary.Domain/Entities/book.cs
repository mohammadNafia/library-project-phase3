namespace MiniLibrary.Domain.Entities;
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
  
    public Genre Genre { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public bool? IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? Changes { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
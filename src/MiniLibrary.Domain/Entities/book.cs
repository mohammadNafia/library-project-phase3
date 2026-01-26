namespace MiniLibrary.Domain.Entities;
public class Book
{
      public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
  
    public string Genre { get; set; } = string.Empty;   
   
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
namespace MiniLibrary.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();


}
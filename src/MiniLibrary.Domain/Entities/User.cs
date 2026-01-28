using Domain.Interfaces;

namespace MiniLibrary.Domain.Entities;

public class User : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? Changes { get; set; }
    public DateTime? UpdatedAt { get; set; }


}
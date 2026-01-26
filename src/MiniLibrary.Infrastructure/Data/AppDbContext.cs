namespace MiniLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MiniLibrary.Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)

{

}

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
   modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


    }
}
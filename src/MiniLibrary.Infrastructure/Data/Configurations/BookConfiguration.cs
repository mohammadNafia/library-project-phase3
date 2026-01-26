
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniLibrary.Domain.Entities;

namespace MiniLibrary.Infrastructure.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(u => u.Id);
        
       builder.Property(b => b.Title)
              .IsRequired()
              .HasMaxLength(200);

        builder.Property(b => b.Genre)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasMany(b => b.Loans)
               .WithOne(l => l.Book)
               .HasForeignKey(l => l.BookId);       
}
}
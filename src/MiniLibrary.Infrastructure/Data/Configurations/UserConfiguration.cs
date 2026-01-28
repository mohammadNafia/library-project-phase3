using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniLibrary.Domain.Entities;

namespace MiniLibrary.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(u => u.Password)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(u => u.Role)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(u => u.IsDeleted)
               .HasDefaultValue(false);

        builder.HasMany(u => u.Loans)
               .WithOne(l => l.User)
               .HasForeignKey(l => l.UserId);
               
        builder.HasQueryFilter(u => u.IsDeleted == false || u.IsDeleted == null);
    }

}

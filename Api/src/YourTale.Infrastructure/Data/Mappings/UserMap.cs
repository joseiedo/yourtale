using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourTale.Domain.Models;

namespace YourTale.Infrastructure.Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.FullName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
        builder.Property(x => x.NickName).HasMaxLength(50);
        builder.Property(x => x.BirthDate).IsRequired();
        builder.Property(x => x.Cep).IsRequired().HasMaxLength(8);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(128);
        builder.Property(x => x.Picture).HasMaxLength(512);

        builder
            .HasIndex(x => x.Email, "IX_User_Email")
            .IsUnique();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourTale.Domain.Models;

namespace YourTale.Infrastructure.Data.Mappings;

public class LikeMap : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.ToTable("Like");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("SMALLDATETIME")
            .HasMaxLength(60)
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.User)
            .WithMany(x => x.Likes)
            .HasConstraintName("FK_Like_User")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Post)
            .WithMany(x => x.Likes)
            .HasConstraintName("FK_Like_Post")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
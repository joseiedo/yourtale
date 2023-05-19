using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourTale.Domain.Models;

namespace YourTale.Infrastructure.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Description).IsRequired().HasMaxLength(255);

        builder.Property(x => x.Picture).IsRequired().HasMaxLength(512);

        builder.Property(x => x.IsPrivate).IsRequired().HasDefaultValue("false");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasMaxLength(60)
            .HasColumnType("SMALLDATETIME");

        builder
            .HasOne(x => x.Author)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Author")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Comments)
            .WithOne(x => x.Post)
            .HasConstraintName("FK_Post_Comments")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
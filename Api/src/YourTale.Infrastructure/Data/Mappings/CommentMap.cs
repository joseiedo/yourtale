using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourTale.Domain.Models;

namespace YourTale.Infrastructure.Data.Mappings;

public class CommentMap : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comment");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("SMALLDATETIME")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Comments)
            .HasConstraintName("FK_Comment_Author")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
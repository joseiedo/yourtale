using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourTale.Domain.Models;

namespace YourTale.Infrastructure.Data.Mappings;

public class FriendRequestMap : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.ToTable("FriendRequest");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();


        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("SMALLDATETIME")
            .HasMaxLength(60)
            .HasDefaultValueSql("GETDATE()");

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.FriendRequestsReceived)
            .HasConstraintName("FK_FriendRequest_User")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Friend)
            .WithMany(x => x.FriendRequestsSent)
            .HasConstraintName("FK_FriendRequest_Friend")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
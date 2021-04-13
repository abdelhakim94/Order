using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("user_refresh_token", "order_schema");

            builder.HasKey(e => e.UserId);

            builder.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(e => e.Token)
                .HasColumnName("token")
                .IsRequired();

            builder.Property(e => e.ExpireAt)
                .HasColumnName("expire_at")
                .IsRequired();

            builder.HasOne(e => e.User)
                .WithOne(e => e.RefreshToken)
                .HasForeignKey<UserRefreshToken>(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("user_profile", "order_schema");

            builder.HasKey(up => new
            {
                up.IdUser,
                up.IdProfile,
            }).HasName("PK_USER_PROFILE");

            builder.Property(up => up.IdUser)
                .HasColumnName("id_user")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(up => up.IdProfile)
                .HasColumnName("id_profile")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(up => up.Profile)
                .WithMany(p => p.UsersProfile)
                .HasForeignKey(up => up.IdProfile)
                .HasConstraintName("FK_USER_PROFILE_PROFILE")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(up => up.User)
                .WithMany(p => p.UserProfiles)
                .HasForeignKey(up => up.IdUser)
                .HasConstraintName("FK_USER_PROFILE_USER")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

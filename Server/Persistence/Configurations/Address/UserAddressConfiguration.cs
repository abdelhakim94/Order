using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("user_address", "order_schema");

            builder.HasKey(ua => new
            {
                ua.IdUser,
                ua.Address1,
                ua.Address2,
                ua.IdCity,
            }).HasName("PK_USER_ADDRESS");

            builder.Property(ua => ua.IdUser)
                .HasColumnName("id_user")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(ua => ua.Address1)
                .HasColumnName("address1")
                .HasColumnType("character varying")
                .IsRequired();

            builder.Property(ua => ua.Address2)
                .HasColumnName("address2")
                .HasColumnType("character varying");

            builder.Property(ua => ua.IdCity)
                .HasColumnName("id_city")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(ua => ua.LastTimeUsed)
                .HasColumnName("last_time_used")
                .HasColumnType("timestamp with time zone");

            builder.HasOne(ua => ua.User)
                .WithMany(u => u.UserAddresses)
                .HasForeignKey(ua => ua.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USER_ADDRESS_USER");

            builder.HasOne(ua => ua.Address)
                .WithMany(a => a.UsersAddress)
                .HasForeignKey(ua => new
                {
                    ua.Address1,
                    ua.Address2,
                    ua.IdCity,
                })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USER_ADDRESS_ADDRESS");
        }
    }
}
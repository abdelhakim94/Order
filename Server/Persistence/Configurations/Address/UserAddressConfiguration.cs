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
                ua.ZipCodeCity,
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

            builder.Property(ua => ua.ZipCodeCity)
                .HasColumnName("zip_code_city")
                .HasColumnType("character varying")
                .HasMaxLength(5)
                .IsRequired();

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
                    ua.ZipCodeCity,
                })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USER_ADDRESS_ADDRESS");
        }
    }
}
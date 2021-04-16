using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("address", "order_schema");

            builder.HasKey(a => new
            {
                a.Address1,
                a.Address2,
                a.ZipCodeCity,
            }).HasName("PK_ADDRESS");

            builder.Property(a => a.Address1)
                .HasColumnName("address1")
                .HasColumnType("character varying")
                .IsRequired();

            builder.Property(a => a.Address2)
                .HasColumnName("address2")
                .HasColumnType("character varying");

            builder.Property(a => a.ZipCodeCity)
                .HasColumnName("zip_code_city")
                .HasColumnType("character varying")
                .HasMaxLength(5)
                .IsRequired();
        }
    }
}

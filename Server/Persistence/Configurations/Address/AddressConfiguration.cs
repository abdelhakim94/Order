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
                a.IdCity,
            }).HasName("PK_ADDRESS");

            builder.Property(a => a.Address1)
                .HasColumnName("address1")
                .HasColumnType("character varying")
                .IsRequired();

            builder.Property(a => a.Address2)
                .HasColumnName("address2")
                .HasColumnType("character varying")
                .HasDefaultValue<string>("");

            builder.Property(a => a.IdCity)
                .HasColumnName("id_city")
                .HasColumnType("integer")
                .IsRequired();
        }
    }
}

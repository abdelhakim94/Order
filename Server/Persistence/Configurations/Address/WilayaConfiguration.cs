using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class WilayaConfiguration : IEntityTypeConfiguration<Wilaya>
    {
        public void Configure(EntityTypeBuilder<Wilaya> builder)
        {
            builder.ToTable("wilaya", "order_schema");

            builder.HasKey(w => w.Code)
                .HasName("PK_WILAYA");

            builder.HasIndex(w => w.ZipCode)
                .IsUnique();

            builder.Property(w => w.Code)
                .HasColumnName("code")
                .HasColumnType("character varying")
                .HasMaxLength(2)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(w => w.ZipCode)
                .HasColumnName("zip_code")
                .HasColumnType("character varying")
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(w => w.Name)
                .HasColumnName("name")
                .HasColumnType("character varying")
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
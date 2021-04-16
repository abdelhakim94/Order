using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("city", "order_schema");

            builder.HasKey(c => c.ZipCode)
                .HasName("PK_CITY");

            builder.HasIndex(c => c.Name)
                .HasDatabaseName("INDEX_NAME_CITY");

            builder.Property(c => c.ZipCode)
                .HasColumnName("zip_code")
                .HasColumnType("character varying")
                .HasMaxLength(5)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("character varying")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(c => c.CodeWilaya)
                .HasColumnName("code_wilaya")
                .HasColumnType("character varying")
                .HasMaxLength(2)
                .IsRequired();

            builder.HasOne(c => c.Wilaya)
                .WithMany(w => w.Cities)
                .HasForeignKey(c => c.CodeWilaya)
                .HasConstraintName("FK_CITY_WILAYA")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Addresses)
                .WithOne(a => a.City)
                .HasForeignKey(a => a.ZipCodeCity)
                .HasConstraintName("FK_ADDRESS_CITY")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

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

            builder.HasKey(c => c.Id)
                .HasName("PK_CITY");

            builder.HasIndex(c => c.Name)
                .HasDatabaseName("INDEX_NAME_CITY");

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("character varying")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(c => c.Latitude)
                .HasColumnName("latitude")
                .HasColumnType("decimal(22, 20)")
                .IsRequired();

            builder.Property(c => c.Longitude)
                .HasColumnName("longitude")
                .HasColumnType("decimal(23, 20)")
                .IsRequired();

            builder.Property(c => c.IdWilaya)
                .HasColumnName("id_wilaya")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(c => c.Wilaya)
                .WithMany(w => w.Cities)
                .HasForeignKey(c => c.IdWilaya)
                .HasConstraintName("FK_CITY_WILAYA")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Addresses)
                .WithOne(a => a.City)
                .HasForeignKey(a => a.IdCity)
                .HasConstraintName("FK_ADDRESS_CITY")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

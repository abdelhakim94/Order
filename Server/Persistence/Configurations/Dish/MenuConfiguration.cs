using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("menu", "order_schema");

            builder.HasKey(d => d.Id)
                .HasName("PK_MENU");

            builder.Property(d => d.Id)
                .HasColumnName("id")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(d => d.Name)
                .HasColumnName("name")
                .HasColumnType("character varying")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(d => d.Description)
                .HasColumnName("description")
                .HasColumnType("character varying")
                .HasMaxLength(200);

            builder.Property(d => d.Picture)
                .HasColumnName("picture")
                .HasColumnType("character varying");

            builder.Property(d => d.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category", "order_schema");

            builder.HasIndex(e => e.Id)
                .HasDatabaseName("category_id_index");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(e => e.Label)
                .HasColumnName("label")
                .HasColumnType("character varying(30)")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(e => e.Picture)
                .HasColumnName("picture")
                .HasColumnType("character varying")
                .IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class ExtraConfiguration : IEntityTypeConfiguration<Extra>
    {
        public void Configure(EntityTypeBuilder<Extra> builder)
        {
            builder.ToTable("extra", "order_schema");

            builder.HasKey(e => e.Id)
                .HasName("PK_EXTRA");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(30)
                .HasColumnType("character varying")
                .IsRequired();

            builder.Property(e => e.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(8, 2)")
                .IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.ToTable("section", "order_schema");

            builder.HasKey(s => s.Id)
                .HasName("PK_SECTION");

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(s => s.Name)
                .HasColumnName("name")
                .HasColumnType("character varying")
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}

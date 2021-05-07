using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class DishSectionConfiguration : IEntityTypeConfiguration<DishSection>
    {
        public void Configure(EntityTypeBuilder<DishSection> builder)
        {
            builder.ToTable("dish_section", "order_schema");

            builder.HasKey(ds => new
            {
                ds.IdDish,
                ds.IdSection,
            }).HasName("PK_DISH_SECTION");

            builder.Property(ds => ds.IdDish)
                .HasColumnName("id_dish")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(ds => ds.IdSection)
                .HasColumnName("id_section")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(ds => ds.IsMandatory)
                .HasColumnName("is_mandatory")
                .HasColumnType("boolean");

            builder.HasOne(ds => ds.Dish)
                .WithMany(d => d.DishSections)
                .HasForeignKey(ds => ds.IdDish)
                .HasConstraintName("FK_DISH_SECTION_DISH")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ds => ds.Section)
                .WithMany(d => d.DishesSection)
                .HasForeignKey(ds => ds.IdSection)
                .HasConstraintName("FK_DISH_SECTION_SECTION")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
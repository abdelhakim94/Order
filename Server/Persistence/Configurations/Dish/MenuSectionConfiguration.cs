using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class MenuSectionConfiguration : IEntityTypeConfiguration<MenuSection>
    {
        public void Configure(EntityTypeBuilder<MenuSection> builder)
        {
            builder.ToTable("menu_section", "order_schema");

            builder.HasKey(ms => new
            {
                ms.IdMenu,
                ms.IdSection,
            }).HasName("PK_MENU_SECTION");

            builder.Property(ms => ms.IdMenu)
                .HasColumnName("id_menu")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(ms => ms.IdSection)
                .HasColumnName("id_section")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(ms => ms.Menu)
                .WithMany(m => m.MenuSections)
                .HasForeignKey(ms => ms.IdMenu)
                .HasConstraintName("FK_MENU_SECTION_MENU")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ms => ms.Section)
                .WithMany(m => m.MenuesSection)
                .HasForeignKey(ms => ms.IdSection)
                .HasConstraintName("FK_MENU_SECTION_SECTION")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

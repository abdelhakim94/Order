using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class MenuExtraConfiguration : IEntityTypeConfiguration<MenuExtra>
    {
        public void Configure(EntityTypeBuilder<MenuExtra> builder)
        {
            builder.ToTable("menu_extra", "order_schema");

            builder.HasKey(mo => new { mo.IdMenu, mo.IdExtra })
                .HasName("PK_MENU_EXTRA");

            builder.Property(mo => mo.IdMenu)
                .HasColumnName("id_menu")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(mo => mo.IdExtra)
                .HasColumnName("id_extra")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(mo => mo.Menu)
                .WithMany(m => m.MenuExtras)
                .HasForeignKey(mo => mo.IdMenu)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(mo => mo.Extra)
                .WithMany(o => o.MenusExtra)
                .HasForeignKey(mo => mo.IdExtra)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

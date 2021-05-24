using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class MenuOptionConfiguration : IEntityTypeConfiguration<MenuOption>
    {
        public void Configure(EntityTypeBuilder<MenuOption> builder)
        {
            builder.ToTable("menu_option", "order_schema");

            builder.HasKey(mo => new { mo.IdMenu, mo.IdOption })
                .HasName("PK_MENU_OPTION");

            builder.Property(mo => mo.IdMenu)
                .HasColumnName("id_menu")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(mo => mo.IdOption)
                .HasColumnName("id_option")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(mo => mo.Menu)
                .WithMany(m => m.MenuOptions)
                .HasForeignKey(mo => mo.IdMenu)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(mo => mo.Option)
                .WithMany(o => o.MenuesOption)
                .HasForeignKey(mo => mo.IdOption)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

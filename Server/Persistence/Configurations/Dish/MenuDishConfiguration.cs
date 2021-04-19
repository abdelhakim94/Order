using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class MenuDishConfiguration : IEntityTypeConfiguration<MenuDish>
    {
        public void Configure(EntityTypeBuilder<MenuDish> builder)
        {
            builder.ToTable("menu_dish", "order_schema");

            builder.HasKey(md => new
            {
                md.IdDish,
                md.IdMenu,
            }).HasName("PK_MENU_DISH");

            builder.Property(md => md.IdDish)
                .HasColumnName("id_dish")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(md => md.IdMenu)
                .HasColumnName("id_menu")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(md => md.Dish)
                .WithMany(d => d.MenuesDish)
                .HasForeignKey(md => md.IdDish)
                .HasConstraintName("FK_MENU_DISH_DISH");

            builder.HasOne(md => md.Menu)
                .WithMany(m => m.MenuDishes)
                .HasForeignKey(md => md.IdMenu)
                .HasConstraintName("FK_MENU_DISH_MENU");
        }
    }
}

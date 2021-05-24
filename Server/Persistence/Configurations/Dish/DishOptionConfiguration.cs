using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class DishOptionConfiguration : IEntityTypeConfiguration<DishOption>
    {
        public void Configure(EntityTypeBuilder<DishOption> builder)
        {
            builder.ToTable("dish_option", "order_schema");

            builder.HasKey(dop => new { dop.IdDish, dop.IdOption })
                .HasName("PK_DISH_OPTION");

            builder.Property(dop => dop.IdDish)
                .HasColumnName("id_dish")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(dop => dop.IdOption)
                .HasColumnName("id_option")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(dop => dop.Dish)
                .WithMany(d => d.DishOptions)
                .HasForeignKey(dop => dop.IdDish)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(dop => dop.Option)
                .WithMany(o => o.DishesOption)
                .HasForeignKey(dop => dop.IdOption)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

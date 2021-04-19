using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class DishCategoryConfiguraion : IEntityTypeConfiguration<DishCategory>
    {
        public void Configure(EntityTypeBuilder<DishCategory> builder)
        {
            builder.ToTable("dish_category", "order_schema");

            builder.HasKey(dc => new
            {
                dc.IdCategory,
                dc.IdDish,
            }).HasName("PK_DISH_CATEGORY");

            builder.Property(dc => dc.IdCategory)
                .HasColumnName("id_category")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(dc => dc.IdDish)
                .HasColumnName("id_dish")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(dc => dc.Category)
                .WithMany(c => c.DishesCategory)
                .HasForeignKey(dc => dc.IdCategory)
                .HasConstraintName("FK_DISH_CATEGORY_CATEGORY")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(dc => dc.Dish)
                .WithMany(d => d.DishCategories)
                .HasForeignKey(dc => dc.IdDish)
                .HasConstraintName("FK_DISH_CATEGORY_DISH")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

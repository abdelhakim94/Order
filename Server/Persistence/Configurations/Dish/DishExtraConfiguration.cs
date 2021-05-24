using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class DishExtraConfiguration : IEntityTypeConfiguration<DishExtra>
    {
        public void Configure(EntityTypeBuilder<DishExtra> builder)
        {
            builder.ToTable("dish_extra", "order_schema");

            builder.HasKey(de => new { de.IdDish, de.IdExtra })
                .HasName("PK_DISH_EXTRA");

            builder.Property(de => de.IdExtra)
                .HasColumnName("id_extra")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(de => de.IdDish)
                .HasColumnName("id_dish")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(de => de.Extra)
                .WithMany(e => e.DishesExtra)
                .HasForeignKey(de => de.IdExtra)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(de => de.Dish)
                .WithMany(d => d.DishExtras)
                .HasForeignKey(de => de.IdDish)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

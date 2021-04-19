using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class CardDishConfiguration : IEntityTypeConfiguration<CardDish>
    {
        public void Configure(EntityTypeBuilder<CardDish> builder)
        {
            builder.ToTable("card_dish", "order_schema");

            builder.HasKey(cd => new
            {
                cd.IdDish,
                cd.IdCard,
            }).HasName("PK_CARD_DISH");

            builder.Property(cd => cd.IdDish)
                .HasColumnName("id_dish")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(cd => cd.IdCard)
                .HasColumnName("id_card")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(cs => cs.Dish)
                .WithMany(d => d.CardsDish)
                .HasForeignKey(cs => cs.IdDish)
                .HasConstraintName("FK_CARD_DISH_DISH");

            builder.HasOne(cs => cs.Card)
                .WithMany(c => c.CardDishes)
                .HasForeignKey(cs => cs.IdCard)
                .HasConstraintName("FK_CARD_DISH_CARD");
        }
    }
}

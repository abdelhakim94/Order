using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class CardMenuConfiguration : IEntityTypeConfiguration<CardMenu>
    {
        public void Configure(EntityTypeBuilder<CardMenu> builder)
        {
            builder.ToTable("card_menu", "order_schema");

            builder.HasKey(cm => new
            {
                cm.IdCard,
                cm.IdMenu,
            }).HasName("PK_CARD_MENU");

            builder.Property(cm => cm.IdCard)
                .HasColumnName("id_card")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(cm => cm.IdMenu)
                .HasColumnName("id_menu")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(cm => cm.Card)
                .WithMany(c => c.CardMenus)
                .HasForeignKey(cm => cm.IdCard)
                .HasConstraintName("FK_CARD_MENU_CARD");

            builder.HasOne(cm => cm.Menu)
                .WithMany(m => m.CardsMenu)
                .HasForeignKey(md => md.IdMenu)
                .HasConstraintName("FK_CARD_MENU_MENU");
        }
    }
}

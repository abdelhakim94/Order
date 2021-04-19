using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class CardSectionConfiguration : IEntityTypeConfiguration<CardSection>
    {
        public void Configure(EntityTypeBuilder<CardSection> builder)
        {
            builder.ToTable("card_section", "order_schema");

            builder.HasKey(cs => new
            {
                cs.IdSection,
                cs.IdCard,
            }).HasName("PK_CARD_SECTION");

            builder.Property(cs => cs.IdSection)
                .HasColumnName("id_section")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(cs => cs.IdCard)
                .HasColumnName("id_card")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(cs => cs.Section)
                .WithMany(s => s.CardsSection)
                .HasForeignKey(cs => cs.IdSection)
                .HasConstraintName("FK_CARD_SECTION_SECTION");

            builder.HasOne(cs => cs.Card)
                .WithMany(c => c.CardSections)
                .HasForeignKey(cs => cs.IdCard)
                .HasConstraintName("FK_CARD_SECTION_CARD");
        }
    }
}

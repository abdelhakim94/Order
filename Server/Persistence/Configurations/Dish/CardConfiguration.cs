using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("card", "order_schema");

            builder.HasKey(c => c.Id)
                .HasName("PK_CARD");

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("character varying")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(c => c.IdUser)
                .HasColumnName("id_user")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(c => c.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("boolean");

            builder.HasOne(c => c.User)
                .WithMany(u => u.Cards)
                .HasForeignKey(c => c.IdUser)
                .HasConstraintName("FK_CARD_USER")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

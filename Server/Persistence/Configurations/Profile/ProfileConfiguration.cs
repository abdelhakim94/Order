using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("profile", "order_schema");

            builder.HasKey(p => p.Id)
                .HasName("PK_PROFILE");

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("integer")
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnName("name")
                .HasColumnType("character varying")
                .IsRequired();
        }
    }
}

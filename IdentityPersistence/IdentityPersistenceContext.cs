using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Order.DomainModel;

namespace Order.IdentityPersistence
{
    public class IdentityPersistenceContext<T>
        : IdentityDbContext<
            User,
            Role,
            int,
            Order.DomainModel.UserClaim,
            UserRole,
            UserLogin,
            RoleClaim,
            UserToken>
        , IIdentityPersistenceContext
        where T : DbContext
    {
        public IdentityPersistenceContext(DbContextOptions<T> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
                .HasAnnotation("Relational:Collation", "English_United States.1252")
                .HasDefaultSchema("order_schema");
        }
    }
}

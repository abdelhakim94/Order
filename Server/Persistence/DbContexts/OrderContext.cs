using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public class OrderContext
        : IdentityDbContext<
            User,
            Role,
            int,
            UserClaim,
            UserRole,
            UserLogin,
            RoleClaim,
            UserToken>
        , IOrderContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

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

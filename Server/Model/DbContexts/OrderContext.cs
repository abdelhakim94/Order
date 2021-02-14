using System.Reflection;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Order.Server.Model
{
    public class OrderContext
        : ApiAuthorizationDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
        , IOrderContext
    {
        public OrderContext(
            DbContextOptions<OrderContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions) { }

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

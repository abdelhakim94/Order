using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Order.IdentityPersistence;

namespace Order.Application.Server.Persistence
{
    public class OrderContext : IdentityPersistenceContext<OrderContext>, IIdentityPersistenceContext, IOrderContext
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

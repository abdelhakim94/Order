using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Order.Server.Domain
{
    public class OrderContext : DbContext, IOrderContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
                .HasAnnotation("Relational:Collation", "English_United States.1252");
        }
    }
}

using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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

        public bool HasChanges() => ChangeTracker.HasChanges();

        public override Task<int> SaveChangesAsync(CancellationToken ct) => base.SaveChangesAsync(ct);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
                .HasAnnotation("Relational:Collation", "English_United States.1252")
                .HasDefaultSchema("order_schema");
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public override DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<UserRefreshToken> UserRefreshToken { get; set; }
        public DbSet<Wilaya> Wilaya { get; set; }
    }
}

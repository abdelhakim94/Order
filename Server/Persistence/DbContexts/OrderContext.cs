using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Order.DomainModel;
using Order.Server.Helpers;

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
            modelBuilder.HasDbFunction(typeof(DistanceHelper).GetMethod(nameof(DistanceHelper.IsNear), new[]
            {
                typeof(int),
                typeof(decimal),
                typeof(decimal),
                typeof(decimal)
            }))
            .HasName("is_near");
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<CardDish> CardDish { get; set; }
        public DbSet<CardMenu> CardMenu { get; set; }
        public DbSet<CardSection> CardSection { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<DishCategory> DishCategory { get; set; }
        public DbSet<DishSection> DishSection { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuDish> MenuDish { get; set; }
        public DbSet<MenuSection> MenuSection { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Section> Section { get; set; }
        public override DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<UserRefreshToken> UserRefreshToken { get; set; }
        public DbSet<Wilaya> Wilaya { get; set; }
    }
}

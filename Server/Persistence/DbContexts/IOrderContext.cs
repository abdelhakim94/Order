using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public interface IOrderContext
    {
        DbSet<Address> Address { get; set; }
        DbSet<Card> Card { get; set; }
        DbSet<CardSection> CardSection { get; set; }
        DbSet<Category> Category { get; set; }
        DbSet<City> City { get; set; }
        DbSet<Dish> Dish { get; set; }
        DbSet<DishCategory> DishCategory { get; set; }
        DbSet<DishExtra> DishExtra { get; set; }
        DbSet<DishOption> DishOption { get; set; }
        DbSet<DishSection> DishSection { get; set; }
        DbSet<Extra> Extra { get; set; }
        DbSet<Menu> Menu { get; set; }
        DbSet<MenuSection> MenuSection { get; set; }
        DbSet<Option> Option { get; set; }
        DbSet<Profile> Profile { get; set; }
        DbSet<Section> Section { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserAddress> UserAddress { get; set; }
        DbSet<UserProfile> UserProfile { get; set; }
        DbSet<UserRefreshToken> UserRefreshToken { get; set; }
        DbSet<Wilaya> Wilaya { get; set; }

        bool HasChanges();
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}

using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public interface IOrderContext
    {
        DbSet<Address> Address { get; set; }
        DbSet<Category> Category { get; set; }
        DbSet<City> City { get; set; }
        DbSet<Profile> Profile { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserAddress> UserAddress { get; set; }
        DbSet<UserProfile> UserProfile { get; set; }
        DbSet<UserRefreshToken> UserRefreshToken { get; set; }
        DbSet<Wilaya> Wilaya { get; set; }

        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}

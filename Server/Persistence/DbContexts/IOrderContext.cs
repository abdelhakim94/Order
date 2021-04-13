using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public interface IOrderContext
    {
        DbSet<Category> Category { get; set; }
        DbSet<UserRefreshToken> UserRefreshToken { get; set; }
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}

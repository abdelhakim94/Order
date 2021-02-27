using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.DomainModel;

namespace Order.Server.Persistence
{
    public interface IOrderContext
    {
        DbSet<UserRefreshToken> UserRefreshToken { get; set; }

        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}

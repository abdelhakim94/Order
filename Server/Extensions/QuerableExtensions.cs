using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.Server.Dto;

namespace Order.Server.Extensions
{
    public static class QuerableExtensions
    {
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            int pageIndex,
            int itemsPerPage,
            CancellationToken ct)
        {
            var results = await source
                .Skip((pageIndex - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            var totalItems = await source.CountAsync(ct);

            return new PaginatedList<T>(results, pageIndex, itemsPerPage, totalItems);
        }
    }
}

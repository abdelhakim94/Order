using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;
using Order.Shared.Dto;

namespace Order.Server.CQRS.User.Queries
{
    public class SearchCitiesQueryHandler : IRequestHandler<SearchCitiesQuery, List<DatalistOption>>
    {
        private readonly IOrderContext context;

        public SearchCitiesQueryHandler(IOrderContext context)
            => this.context = context;

        public async Task<List<DatalistOption>> Handle(SearchCitiesQuery query, CancellationToken ct)
        {
            return await context.City
                .Where(c => c.Name.Contains(query.Search.ToUpper()))
                .OrderBy(c => c.Name)
                .Select(c => new DatalistOption
                {
                    Id = c.ZipCode,
                    Value = c.Name,
                })
                .ToListAsync();
        }
    }
}

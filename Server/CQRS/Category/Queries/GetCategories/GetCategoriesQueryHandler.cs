using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;
using Order.Shared.Dto.Category;

namespace Order.Server.CQRS.Category.Queries
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryListItemDto>>
    {
        private readonly IOrderContext context;

        public GetCategoriesQueryHandler(IOrderContext context) => this.context = context;

        public async Task<List<CategoryListItemDto>> Handle(GetCategoriesQuery query, CancellationToken ct)
        {
            return await context.Category
                .Select(c => new CategoryListItemDto
                {
                    Id = c.Id,
                    Label = c.Label,
                    Picture = c.Picture,
                    IsMain = c.IsMain,
                })
                .ToListAsync();
        }
    }
}

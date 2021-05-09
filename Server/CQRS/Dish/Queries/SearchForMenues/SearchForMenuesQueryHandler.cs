using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Order.Server.Dto;
using Order.Server.Extensions;
using Order.Server.Helpers;
using Order.Server.Persistence;
using Order.Shared.Dto;
using Order.Shared.Dto.Dish;

namespace Order.Server.CQRS.Dish.Queries
{
    public class SearchForMenuesQueryHandler : IRequestHandler<SearchForMenuesQuery, PaginatedList<MenuDetailsDto>>
    {
        private readonly IOrderContext context;
        private readonly DistanceConfig distanceConfig;

        public SearchForMenuesQueryHandler(IOrderContext context, DistanceConfig distanceConfig)
        {
            this.context = context;
            this.distanceConfig = distanceConfig;
        }

        public async Task<PaginatedList<MenuDetailsDto>> Handle(SearchForMenuesQuery query, CancellationToken ct)
        {
            var filter = query.Filter;
            var dbQuery = context.Menu.AsQueryable<DomainModel.Menu>();
            dbQuery = ApplyFilter(dbQuery, filter);

            return await dbQuery.Where(m =>
                            m.CardsMenu.Any(cm => cm.Card.IsActive && DistanceHelper.IsNear(
                                    cm.Card.User.Id,
                                    filter.Latitude,
                                    filter.Longitude,
                                    distanceConfig.MinDistance))
                         || m.MenuSections
                                .Where(ms => !ms.MenuOwns)
                                .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                                .Any(cs => cs.Card.IsActive && DistanceHelper.IsNear(
                                    cs.Card.User.Id,
                                    filter.Latitude,
                                    filter.Longitude,
                                    distanceConfig.MinDistance))
                )
                .Select(m => new MenuDetailsDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Picture = m.Picture,
                    Price = m.Price,
                    ChefFullName = m.CardsMenu
                            .Select(cm => $"{cm.Card.User.FirstName} {cm.Card.User.LastName}")
                            .FirstOrDefault() ??
                        m.MenuSections
                            .Where(ms => !ms.MenuOwns)
                            .SelectMany(ms => ms.Section.CardsSection, (ds, cs) => cs)
                            .Select(cs => $"{cs.Card.User.FirstName} {cs.Card.User.LastName}")
                            .FirstOrDefault()
                })
                .ToPaginatedListAsync(filter.PageIndex, filter.ItemsPerPage, ct);
        }

        private IQueryable<DomainModel.Menu> ApplyFilter(IQueryable<DomainModel.Menu> source, MenuesSearchFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                filter.Search = filter.Search.ToLowerInvariant();
                source = source.Where(m => m.Name.ToLower().Contains(filter.Search)
                    || m.Description.ToLower().Contains(filter.Search)
                    || m.MenuDishes.Any(md => md.Dish.Name.ToLower().Contains(filter.Search)
                        || md.Dish.Description.ToLower().Contains(filter.Search)
                        || md.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(filter.Search)))
                    || m.MenuSections.Where(m => m.MenuOwns).Any(ms => ms.Section.DishesSection
                        .Any(ds => ds.Dish.Name.ToLower().Contains(filter.Search)
                            || ds.Dish.Description.ToLower().Contains(filter.Search)
                            || ds.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(filter.Search)))));
            }

            return source;
        }
    }
}

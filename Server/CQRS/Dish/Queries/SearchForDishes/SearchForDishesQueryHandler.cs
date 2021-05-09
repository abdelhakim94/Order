using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Order.Server.Dto;
using Order.Server.Helpers;
using Order.Server.Persistence;
using Order.Shared.Dto;
using Order.Server.Extensions;
using Order.Shared.Dto.Dish;

namespace Order.Server.CQRS.Dish.Queries
{
    public class SearchForDishesQueryHandler : IRequestHandler<SearchForDishesQuery, PaginatedList<DishDetailsDto>>
    {
        private readonly IOrderContext context;
        private readonly DistanceConfig distanceConfig;

        public SearchForDishesQueryHandler(IOrderContext context, DistanceConfig distanceConfig)
        {
            this.context = context;
            this.distanceConfig = distanceConfig;
        }

        public async Task<PaginatedList<DishDetailsDto>> Handle(SearchForDishesQuery query, CancellationToken ct)
        {
            var filter = query.Filter;
            var dbQuery = context.Dish.Where(d => !d.IsMenuOnly);
            dbQuery = ApplyFilter(dbQuery, filter);

            return await dbQuery.Where(d =>
                            d.CardsDish
                                .Any(cd => cd.Card.IsActive && DistanceHelper.IsNear(
                                    cd.Card.User.Id,
                                    filter.Latitude,
                                    filter.Longitude,
                                    distanceConfig.MinDistance))
                         || d.DishSections
                                .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                                .Any(cs => cs.Card.IsActive && DistanceHelper.IsNear(
                                    cs.Card.User.Id,
                                    filter.Latitude,
                                    filter.Longitude,
                                    distanceConfig.MinDistance))
                         || d.MenuesDish
                                .SelectMany(md => md.Menu.CardsMenu, (md, cm) => cm)
                                .Any(mc => mc.Card.IsActive && DistanceHelper.IsNear(
                                    mc.Card.User.Id,
                                    filter.Latitude,
                                    filter.Longitude,
                                    distanceConfig.MinDistance))
                         || d.DishSections
                                .SelectMany(ds => ds.Section.MenuesSection, (ds, ms) => ms)
                                .Where(ms => ms.MenuOwns)
                                .SelectMany(ms => ms.Menu.CardsMenu, (ms, cm) => cm)
                                .Any(cm => cm.Card.IsActive && DistanceHelper.IsNear(
                                    cm.Card.User.Id,
                                    filter.Latitude,
                                    filter.Longitude,
                                    distanceConfig.MinDistance))
                )
                .Select(d => new DishDetailsDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Picture = d.Picture,
                    Price = d.Price,
                    ChefFullName = d.CardsDish
                            .Select(cd => $"{cd.Card.User.FirstName} {cd.Card.User.LastName}")
                            .FirstOrDefault() ??
                        d.DishSections
                            .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                            .Select(cs => $"{cs.Card.User.FirstName} {cs.Card.User.LastName}")
                            .FirstOrDefault() ??
                        d.MenuesDish
                            .SelectMany(md => md.Menu.CardsMenu, (md, cm) => cm)
                            .Select(cs => $"{cs.Card.User.FirstName} {cs.Card.User.LastName}")
                            .FirstOrDefault() ??
                        d.DishSections
                            .SelectMany(ds => ds.Section.MenuesSection, (ds, ms) => ms)
                            .Where(ms => ms.MenuOwns)
                            .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                            .Select(cs => $"{cs.Card.User.FirstName} {cs.Card.User.LastName}")
                            .FirstOrDefault(),
                })
                .ToPaginatedListAsync(filter.PageIndex, filter.ItemsPerPage, ct);
        }

        private IQueryable<DomainModel.Dish> ApplyFilter(IQueryable<DomainModel.Dish> source, DishesSearchFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                filter.Search = filter.Search.ToLowerInvariant();
                source = source.Where(d => d.Name.ToLower().Contains(filter.Search)
                    || d.Description.ToLower().Contains(filter.Search)
                    || d.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(filter.Search)));
            }

            return source;
        }
    }
}

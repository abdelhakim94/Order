using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Order.Server.Constants;
using Order.Server.Dto;
using Order.Server.Extensions;
using Order.Server.Helpers;
using Order.Server.Persistence;
using Order.Shared.Dto.Chef;
using Order.Shared.Security.Claims;

namespace Order.Server.CQRS.Chef.Queries
{
    public class SearchForChefsQueryHandler : IRequestHandler<SearchForChefsQuery, PaginatedList<ChefListItemDto>>
    {
        private IOrderContext context;
        private DistanceConfig distanceConfig;

        public SearchForChefsQueryHandler(IOrderContext context, DistanceConfig distanceConfig)
        {
            this.context = context;
            this.distanceConfig = distanceConfig;
        }

        public async Task<PaginatedList<ChefListItemDto>> Handle(SearchForChefsQuery query, CancellationToken ct)
        {
            var filter = query.Filter;

            var dbQuery = context.Users
                .Where(u => u.UserProfiles.Any(p => p.IdProfile == (int)Profile.CHEF)
                    && DatabaseFunctions.IsNear(u.Id, filter.Latitude, filter.Longitude, distanceConfig.MinDistance));

            dbQuery = ApplyFilters(dbQuery, filter);

            return await dbQuery.Select(u => new ChefListItemDto
            {
                Id = u.Id,
                Picture = u.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                ChefFullName = u.FirstName + " " + u.LastName,
                City = u.UserAddresses
                    .OrderBy(ua => ua.LastTimeUsed)
                    .FirstOrDefault()
                    .Address
                    .City
                    .Name,
                Categories = DatabaseFunctions.UserCategories(u.Id),
            })
            .ToPaginatedListAsync(filter.PageIndex, filter.ItemsPerPage, ct);
        }

        private IQueryable<DomainModel.User> ApplyFilters(IQueryable<DomainModel.User> query, ChefsSearchFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Search))
            {
                var search = filter.Search.ToLowerInvariant();

                query = query.Where(u => u.FirstName.ToLower().Contains(search)
                    || u.LastName.ToLower().Contains(search)
                    || search.Contains(u.FirstName.ToLower())
                    || search.Contains(u.LastName.ToLower())
                    || u.Cards.Any(c => c.IsActive && (c.Name.ToLower().Contains(search)
                        || c.CardSections.Any(cs => cs.Section.Name.ToLower().Contains(search)
                            || cs.Section.MenuesSection.Any(ms => !ms.MenuOwns && (ms.Menu.Name.ToLower().Contains(search)
                                || ms.Menu.Description.ToLower().Contains(search)
                                || ms.Menu.MenuOptions.Any(mo => mo.Option.Name.ToLower().Contains(search))
                                || ms.Menu.MenuExtras.Any(me => me.Extra.Name.ToLower().Contains(search))
                                || ms.Menu.MenuSections.Any(ms => ms.MenuOwns && (ms.Section.Name.ToLower().Contains(search)
                                    || ms.Section.DishesSection.Any(ds => ds.Dish.Name.ToLower().Contains(search)
                                        || ds.Dish.Description.ToLower().Contains(search)
                                        || ds.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search))
                                        || ds.Dish.DishOptions.Any(dop => dop.Option.Name.ToLower().Contains(search))
                                        || ds.Dish.DishExtras.Any(de => de.Extra.Name.ToLower().Contains(search)))))))
                            || cs.Section.DishesSection.Any(ds => ds.Dish.Name.ToLower().Contains(search)
                                || ds.Dish.Description.ToLower().Contains(search)
                                || ds.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search))
                                || ds.Dish.DishOptions.Any(dop => dop.Option.Name.ToLower().Contains(search))
                                || ds.Dish.DishExtras.Any(de => de.Extra.Name.ToLower().Contains(search)))))));
            }

            return query;
        }
    }
}

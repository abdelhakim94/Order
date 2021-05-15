using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Order.Server.Dto;
using Order.Server.Extensions;
using Order.Server.Helpers;
using Order.Server.Persistence;
using Order.Shared.Dto.Chef;
using Order.Shared.Security.Claims;

namespace Order.Server.CQRS.Chef.Queries
{
    public class SearchForChefsQueryHandler : IRequestHandler<SearchForChefsQuery, PaginatedList<ChefDetailsDto>>
    {
        private IOrderContext context;
        private DistanceConfig distanceConfig;

        public SearchForChefsQueryHandler(IOrderContext context, DistanceConfig distanceConfig)
        {
            this.context = context;
            this.distanceConfig = distanceConfig;
        }

        public async Task<PaginatedList<ChefDetailsDto>> Handle(SearchForChefsQuery query, CancellationToken ct)
        {
            var filter = query.Filter;

            var dbQuery = context.Users
                .Where(u => u.UserProfiles.Any(p => p.IdProfile == (int)Profile.CHEF)
                    && DatabaseFunctions.IsNear(u.Id, filter.Latitude, filter.Longitude, distanceConfig.MinDistance));

            dbQuery = ApplyFilters(dbQuery, filter);

            return await dbQuery.Select(u => new ChefDetailsDto
            {
                Id = u.Id,
                Picture = u.Picture,
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
                        || c.CardDishes.Any(cd => cd.Dish.Name.ToLower().Contains(search)
                            || cd.Dish.Description.ToLower().Contains(search)
                            || cd.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search)))
                        || c.CardMenus.Any(cm => cm.Menu.Name.ToLower().Contains(search)
                            || cm.Menu.Description.ToLower().Contains(search)
                            || cm.Menu.MenuDishes.Any(md => md.Dish.Name.ToLower().Contains(search)
                                || md.Dish.Description.ToLower().Contains(search)
                                || md.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search)))
                            || cm.Menu.MenuSections.Any(ms => ms.MenuOwns && (ms.Section.Name.ToLower().Contains(search)
                                || ms.Section.DishesSection.Any(ds => ds.Dish.Name.ToLower().Contains(search)
                                    || ds.Dish.Description.ToLower().Contains(search)
                                    || ds.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search))))))
                        || c.CardSections.Any(cs => cs.Section.Name.ToLower().Contains(search)
                            || cs.Section.MenuesSection.Any(ms => !ms.MenuOwns && (ms.Menu.Name.ToLower().Contains(search)
                                || ms.Menu.Description.ToLower().Contains(search)
                                || ms.Menu.MenuDishes.Any(md => md.Dish.Name.ToLower().Contains(search)
                                    || md.Dish.Description.ToLower().Contains(search)
                                    || md.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search)))
                                || ms.Menu.MenuSections.Any(ms => ms.MenuOwns && (ms.Section.Name.ToLower().Contains(search)
                                    || ms.Section.DishesSection.Any(ds => ds.Dish.Name.ToLower().Contains(search)
                                        || ds.Dish.Description.ToLower().Contains(search)
                                        || ds.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search)))))))
                            || cs.Section.DishesSection.Any(ds => ds.Dish.Name.ToLower().Contains(search)
                                || ds.Dish.Description.ToLower().Contains(search)
                                || ds.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search)))))));
            }

            return query;
        }
    }
}

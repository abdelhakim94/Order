using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Constants;
using Order.Server.Dto;
using Order.Server.Extensions;
using Order.Server.Helpers;
using Order.Server.Persistence;
using Order.Shared.Dto.Dish;

namespace Order.Server.CQRS.Dish.Queries
{
    public class SearchForDishesOrMenuesQueryHandler : IRequestHandler<SearchForDishesOrMenuesQuery, PaginatedList<DishOrMenuListItemDto>>
    {
        private readonly IOrderContext context;
        private readonly DistanceConfig distanceConfig;

        public SearchForDishesOrMenuesQueryHandler(IOrderContext context, DistanceConfig distanceConfig)
        {
            this.context = context;
            this.distanceConfig = distanceConfig;
        }

        public async Task<PaginatedList<DishOrMenuListItemDto>> Handle(SearchForDishesOrMenuesQuery query, CancellationToken ct)
        {
            var filter = query.Filter;

            switch (query.DishOrMenu)
            {
                case SearchDishOrMenu.DISHES:
                    return await LoadDishes(filter, ct);
                case SearchDishOrMenu.MENUES:
                    return await LoadMenues(filter, ct);
                case SearchDishOrMenu.DISHES_AND_MENUES:
                    return await LoadDishesAndMenues(filter, ct);
                default:
                    throw new InvalidOperationException($"The value {query.DishOrMenu} is not supported");
            }
        }

        private async Task<PaginatedList<DishOrMenuListItemDto>> LoadDishesAndMenues(DishesOrMenuesSearchFilter filter, CancellationToken ct)
        {
            var dishQuery = BuildDishesQuery(filter);
            var menuQuery = BuildMenuesQuery(filter);

            var dishes = dishQuery.Select(d => new
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Picture = d.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                Price = d.Price,
                IsMenu = false,

                ChefFullName = d.CardsDish
                        .Select(cd => cd.Card.User.FirstName + " " + cd.Card.User.LastName)
                        .FirstOrDefault() ??
                    d.DishSections
                        .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                        .FirstOrDefault() ??
                    d.MenuesDish
                        .SelectMany(md => md.Menu.CardsMenu, (md, cm) => cm)
                        .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                        .FirstOrDefault() ??
                    d.DishSections
                        .SelectMany(ds => ds.Section.MenuesSection, (ds, ms) => ms)
                        .Where(ms => ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                        .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                        .FirstOrDefault(),

                ChefCity = d.CardsDish
                        .Select(cd => cd.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault() ??
                    d.DishSections
                        .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault() ??
                    d.MenuesDish
                        .SelectMany(md => md.Menu.CardsMenu, (md, cm) => cm)
                        .Select(cs => cs.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault() ??
                    d.DishSections
                        .SelectMany(ds => ds.Section.MenuesSection, (ds, ms) => ms)
                        .Where(ms => ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                        .Select(cs => cs.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault(),
            });

            var menues = menuQuery.Select(m => new
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Picture = m.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                Price = m.Price,
                IsMenu = true,

                ChefFullName = m.CardsMenu
                        .Select(cm => cm.Card.User.FirstName + " " + cm.Card.User.LastName)
                        .FirstOrDefault() ??
                    m.MenuSections
                        .Where(ms => !ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                        .FirstOrDefault(),

                ChefCity = m.CardsMenu
                        .Select(cm => cm.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault() ??
                    m.MenuSections
                        .Where(ms => !ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault()
            });

            return await dishes.Union(menues)
                .Select(e => new DishOrMenuListItemDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Picture = e.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                    Price = e.Price,
                    IsMenu = e.IsMenu,
                    ChefFullName = e.ChefFullName,
                    ChefCity = e.ChefCity,
                })
                .ToPaginatedListAsync(filter.PageIndex, filter.ItemsPerPage, ct);
        }

        private async Task<PaginatedList<DishOrMenuListItemDto>> LoadMenues(DishesOrMenuesSearchFilter filter, CancellationToken ct)
        {
            var query = BuildMenuesQuery(filter);
            return await query
            .Select(m => new DishOrMenuListItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Picture = m.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                Price = m.Price,
                IsMenu = true,

                ChefFullName = m.CardsMenu
                        .Select(cm => cm.Card.User.FirstName + " " + cm.Card.User.LastName)
                        .FirstOrDefault() ??
                    m.MenuSections
                        .Where(ms => !ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                        .FirstOrDefault(),

                ChefCity = m.CardsMenu
                        .Select(cm => cm.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault() ??
                    m.MenuSections
                        .Where(ms => !ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault()
            })
            .ToPaginatedListAsync(filter.PageIndex, filter.ItemsPerPage, ct);
        }

        private async Task<PaginatedList<DishOrMenuListItemDto>> LoadDishes(DishesOrMenuesSearchFilter filter, CancellationToken ct)
        {
            var query = BuildDishesQuery(filter);
            return await query.Select(d => new DishOrMenuListItemDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Picture = d.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                Price = d.Price,
                IsMenu = false,

                ChefFullName = d.CardsDish
                        .Select(cd => cd.Card.User.FirstName + " " + cd.Card.User.LastName)
                        .FirstOrDefault() ??
                    d.DishSections
                        .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                        .FirstOrDefault() ??
                    d.MenuesDish
                        .SelectMany(md => md.Menu.CardsMenu, (md, cm) => cm)
                        .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                        .FirstOrDefault() ??
                    d.DishSections
                        .SelectMany(ds => ds.Section.MenuesSection, (ds, ms) => ms)
                        .Where(ms => ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                        .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                        .FirstOrDefault(),

                ChefCity = d.CardsDish
                        .Select(cd => cd.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault() ??
                    d.DishSections
                        .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault() ??
                    d.MenuesDish
                        .SelectMany(md => md.Menu.CardsMenu, (md, cm) => cm)
                        .Select(cs => cs.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault() ??
                    d.DishSections
                        .SelectMany(ds => ds.Section.MenuesSection, (ds, ms) => ms)
                        .Where(ms => ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                        .Select(cs => cs.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault(),
            })
            .ToPaginatedListAsync(filter.PageIndex, filter.ItemsPerPage, ct);
        }

        private IQueryable<DomainModel.Dish> BuildDishesQuery(DishesOrMenuesSearchFilter filter)
        {
            var dbQuery = context.Dish.Where(d => !d.IsMenuOnly);

            dbQuery = ApplyFilter(dbQuery, filter);

            return dbQuery.Where(d =>
                    d.CardsDish
                    .Any(cd => cd.Card.IsActive && DatabaseFunctions.IsNear(
                        cd.Card.User.Id,
                        filter.Latitude,
                        filter.Longitude,
                        distanceConfig.MinDistance))
                || d.DishSections
                    .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                    .Any(cs => cs.Card.IsActive && DatabaseFunctions.IsNear(
                        cs.Card.User.Id,
                        filter.Latitude,
                        filter.Longitude,
                        distanceConfig.MinDistance))
                || d.MenuesDish
                    .SelectMany(md => md.Menu.CardsMenu, (md, cm) => cm)
                    .Any(mc => mc.Card.IsActive && DatabaseFunctions.IsNear(
                        mc.Card.User.Id,
                        filter.Latitude,
                        filter.Longitude,
                        distanceConfig.MinDistance))
                || d.DishSections
                    .SelectMany(ds => ds.Section.MenuesSection, (ds, ms) => ms)
                    .Where(ms => ms.MenuOwns)
                    .SelectMany(ms => ms.Menu.CardsMenu, (ms, cm) => cm)
                    .Any(cm => cm.Card.IsActive && DatabaseFunctions.IsNear(
                        cm.Card.User.Id,
                        filter.Latitude,
                        filter.Longitude,
                        distanceConfig.MinDistance)));

            IQueryable<DomainModel.Dish> ApplyFilter(IQueryable<DomainModel.Dish> source, DishesOrMenuesSearchFilter filter)
            {
                if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                    var search = filter.Search.ToLowerInvariant();
                    source = source.Where(d => d.Name.ToLower().Contains(search)
                        || d.Description.ToLower().Contains(search)
                        || d.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search))
                        || d.DishOptions.Any(dop => dop.Option.Name.ToLower().Contains(search))
                        || d.DishExtras.Any(de => de.Extra.Name.ToLower().Contains(search)));
                }

                return source;
            }
        }

        private IQueryable<DomainModel.Menu> BuildMenuesQuery(DishesOrMenuesSearchFilter filter)
        {
            var dbQuery = context.Menu.AsQueryable<DomainModel.Menu>();

            dbQuery = ApplyFilter(dbQuery, filter);

            return dbQuery.Where(m =>
                m.CardsMenu.Any(cm => cm.Card.IsActive && DatabaseFunctions.IsNear(
                        cm.Card.User.Id,
                        filter.Latitude,
                        filter.Longitude,
                        distanceConfig.MinDistance))
                || m.MenuSections
                    .Where(ms => !ms.MenuOwns)
                    .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                    .Any(cs => cs.Card.IsActive && DatabaseFunctions.IsNear(
                        cs.Card.User.Id,
                        filter.Latitude,
                        filter.Longitude,
                        distanceConfig.MinDistance)));

            IQueryable<DomainModel.Menu> ApplyFilter(IQueryable<DomainModel.Menu> source, DishesOrMenuesSearchFilter filter)
            {
                if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                    var search = filter.Search.ToLowerInvariant();
                    source = source.Where(m => m.Name.ToLower().Contains(search)
                        || m.Description.ToLower().Contains(search)
                        || m.MenuDishes.Any(md => md.Dish.Name.ToLower().Contains(search)
                            || md.Dish.Description.ToLower().Contains(search)
                            || md.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search))
                            || md.Dish.DishOptions.Any(dop => dop.Option.Name.ToLower().Contains(search))
                            || md.Dish.DishExtras.Any(de => de.Extra.Name.ToLower().Contains(search)))
                        || m.MenuSections.Where(m => m.MenuOwns).Any(ms => ms.Section.DishesSection
                            .Any(ds => ds.Dish.Name.ToLower().Contains(search)
                                || ds.Dish.Description.ToLower().Contains(search)
                                || ds.Dish.DishCategories.Any(dc => dc.Category.Label.ToLower().Contains(search))
                                || ds.Dish.DishOptions.Any(dop => dop.Option.Name.ToLower().Contains(search))
                                || ds.Dish.DishExtras.Any(de => de.Extra.Name.ToLower().Contains(search))))
                        || m.MenuOptions.Any(mo => mo.Option.Name.ToLower().Contains(search))
                        || m.MenuExtras.Any(me => me.Extra.Name.ToLower().Contains(search)));
                }

                return source;
            }
        }
    }
}

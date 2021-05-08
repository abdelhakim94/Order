using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Order.Server.Dto;
using Order.Server.Helpers;
using Order.Server.Persistence;
using Order.Shared.Dto;

namespace Order.Server.CQRS.Dish.Queries
{
    public class SearchForDishesQueryHandler : IRequestHandler<SearchForDishesQuery, List<DishDetailsDto>>
    {
        private readonly IOrderContext context;
        private readonly DistanceConfig distanceConfig;

        public SearchForDishesQueryHandler(IOrderContext context, DistanceConfig distanceConfig)
        {
            this.context = context;
            this.distanceConfig = distanceConfig;
        }

        public async Task<List<DishDetailsDto>> Handle(SearchForDishesQuery query, CancellationToken ct)
        {
            return await context.Dish.Where(d => !d.IsMenuOnly
                    && (
                            d.CardsDish
                                .Any(cd => cd.Card.IsActive && DistanceHelper.IsNear(
                                    cd.Card.User.Id,
                                    query.Filter.Latitude,
                                    query.Filter.Longitude,
                                    distanceConfig.MinDistance))
                         || d.DishSections
                                .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                                .Any(cs => cs.Card.IsActive && DistanceHelper.IsNear(
                                    cs.Card.User.Id,
                                    query.Filter.Latitude,
                                    query.Filter.Longitude,
                                    distanceConfig.MinDistance))
                         || d.MenuesDish
                                .SelectMany(md => md.Menu.CardsMenu, (md, cm) => cm)
                                .Any(mc => mc.Card.IsActive && DistanceHelper.IsNear(
                                    mc.Card.User.Id,
                                    query.Filter.Latitude,
                                    query.Filter.Longitude,
                                    distanceConfig.MinDistance))
                         || d.DishSections
                                .SelectMany(ds => ds.Section.MenuesSection, (ds, ms) => ms)
                                .SelectMany(ms => ms.Menu.CardsMenu, (ms, cm) => cm)
                                .Any(cm => cm.Card.IsActive && DistanceHelper.IsNear(
                                    cm.Card.User.Id,
                                    query.Filter.Latitude,
                                    query.Filter.Longitude,
                                    distanceConfig.MinDistance))
                    )
                ).Select(d => new DishDetailsDto
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
                            .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                            .Select(cs => $"{cs.Card.User.FirstName} {cs.Card.User.LastName}")
                            .FirstOrDefault(),
                })
                .ToListAsync();
        }
    }
}

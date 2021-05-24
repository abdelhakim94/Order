using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;
using Order.Shared.Dto.Dish;

namespace Order.Server.CQRS.Dish.Queries
{
    public class GetDishDetailsQueryHandler : IRequestHandler<GetDishDetailsQuery, DishDetailsDto>
    {
        private IOrderContext context;

        public GetDishDetailsQueryHandler(IOrderContext context) => this.context = context;

        public async Task<DishDetailsDto> Handle(GetDishDetailsQuery query, CancellationToken ct)
        {
            return await context.Dish
                .Select(d => new DishDetailsDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Picture = d.Picture,
                    Price = d.Price,

                    ChefId = d.CardsDish
                        .Select(cd => cd.Card.User.Id)
                        .Union(d.DishSections
                            .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                            .Select(cs => cs.Card.User.Id)
                            .Union(d.MenuesDish
                                .SelectMany(md => md.Menu.CardsMenu, (md, cm) => cm)
                                .Select(cs => cs.Card.User.Id)
                                .Union(d.DishSections
                                    .SelectMany(ds => ds.Section.MenuesSection, (ds, ms) => ms)
                                    .Where(ms => ms.MenuOwns)
                                    .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                                    .Select(cs => cs.Card.User.Id))))
                        .Single(id => id != 0),

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

                    Options = d.DishOptions
                        .Select(dop => new OptionDetailsDto
                        {
                            Id = dop.Option.Id,
                            Name = dop.Option.Name,
                        })
                        .ToList(),

                    Extras = d.DishExtras
                        .Select(de => new ExtraDetailsDto
                        {
                            Id = de.Extra.Id,
                            Name = de.Extra.Name,
                            Price = de.Extra.Price,
                        })
                        .ToList(),
                })
                .FirstOrDefaultAsync(d => d.Id == query.Id);
        }
    }
}

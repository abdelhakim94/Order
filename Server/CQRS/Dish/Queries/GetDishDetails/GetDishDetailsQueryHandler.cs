using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Constants;
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
                .Where(d => d.Id == query.Id)
                .Select(d => new DishDetailsDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Picture = d.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                    Price = d.Price,

                    ChefId = d.DishSections
                        .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.Id)
                        .Union(d.DishSections
                            .SelectMany(ds => ds.Section.MenuesSection.Where(ms => ms.MenuOwns), (ds, ms) => ms)
                            .SelectMany(ms => ms.Menu.MenuSections.Where(ms => !ms.MenuOwns), (ms1, ms2) => ms2)
                            .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                            .Select(cs => cs.Card.User.Id))
                        .Single(id => id != 0),

                    ChefFullName = d.DishSections
                            .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                            .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                            .Union(d.DishSections
                                .SelectMany(ds => ds.Section.MenuesSection.Where(ms => ms.MenuOwns), (ds, ms) => ms)
                                .SelectMany(ms => ms.Menu.MenuSections.Where(ms => !ms.MenuOwns), (ms1, ms2) => ms2)
                                .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                                .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName))
                            .Single(),

                    ChefCity = d.DishSections
                            .SelectMany(ds => ds.Section.CardsSection, (ds, cs) => cs)
                            .Select(cs => cs.Card.User.UserAddresses
                                .OrderByDescending(ua => ua.LastTimeUsed)
                                .FirstOrDefault()
                                .Address
                                .City
                                .Name)
                            .Union(d.DishSections
                                .SelectMany(ds => ds.Section.MenuesSection.Where(ms => ms.MenuOwns), (ds, ms) => ms)
                                .SelectMany(ms => ms.Menu.MenuSections.Where(ms => !ms.MenuOwns), (ms1, ms2) => ms2)
                                .SelectMany(ms => ms.Section.CardsSection, (ms, cs) => cs)
                                .Select(cs => cs.Card.User.UserAddresses
                                    .OrderByDescending(ua => ua.LastTimeUsed)
                                    .FirstOrDefault()
                                    .Address
                                    .City
                                    .Name))
                            .Single(),

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
                .SingleOrDefaultAsync();
        }
    }
}

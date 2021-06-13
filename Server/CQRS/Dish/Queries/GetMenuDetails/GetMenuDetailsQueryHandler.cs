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
    public class GetMenuDetailsQueryHandler : IRequestHandler<GetMenuDetailsQuery, MenuDetailsDto>
    {
        private IOrderContext context;

        public GetMenuDetailsQueryHandler(IOrderContext context) => this.context = context;

        public async Task<MenuDetailsDto> Handle(GetMenuDetailsQuery query, CancellationToken ct)
        {
            return await context.Menu
                .Select(m => new MenuDetailsDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Picture = m.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                    Price = m.Price,

                    ChefId = m.MenuSections
                        .Where(ms => !ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.Id)
                        .Single(id => id != 0),

                    ChefFullName = m.MenuSections
                        .Where(ms => !ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.FirstName + " " + cs.Card.User.LastName)
                        .FirstOrDefault(),

                    ChefCity = m.MenuSections
                        .Where(ms => !ms.MenuOwns)
                        .SelectMany(ms => ms.Section.CardsSection, (ds, cs) => cs)
                        .Select(cs => cs.Card.User.UserAddresses
                            .OrderByDescending(ua => ua.LastTimeUsed)
                            .FirstOrDefault()
                            .Address
                            .City
                            .Name)
                        .FirstOrDefault(),

                    Sections = m.MenuSections
                        .Where(ms => ms.MenuOwns)
                        .Select(ms => new SectionDto<MenuDishListItem>
                        {
                            Id = ms.Section.Id,
                            Name = ms.Section.Name,
                            Items = ms.Section.DishesSection.Select(ds => new MenuDishListItem
                            {
                                Id = ds.Dish.Id,
                                Name = ds.Dish.Name,
                                Picture = ds.Dish.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                            }).ToList(),
                        })
                        .ToList(),

                    Options = m.MenuOptions.Select(mo => new OptionDetailsDto
                    {
                        Id = mo.Option.Id,
                        Name = mo.Option.Name,
                    })
                    .ToList(),

                    Extras = m.MenuExtras.Select(me => new ExtraDetailsDto
                    {
                        Id = me.Extra.Id,
                        Name = me.Extra.Name,
                        Price = me.Extra.Price,
                    })
                    .ToList(),
                })
                .FirstOrDefaultAsync(m => m.Id == query.Id);
        }
    }
}

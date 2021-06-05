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

                    ChefId = m.CardsMenu
                        .Select(cm => cm.Card.User.Id)
                        .Union(m.MenuSections
                            .Where(ms => !ms.MenuOwns)
                            .SelectMany(ms => ms.Section.CardsSection, (ds, cs) => cs)
                            .Select(cs => cs.Card.User.Id))
                        .Single(id => id != 0),

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
                            .FirstOrDefault(),

                    Dishes = m.MenuDishes.Select(md => new MenuDishListItem
                    {
                        Id = md.Dish.Id,
                        Name = md.Dish.Name,
                        Description = md.Dish.Description,
                        Picture = md.Dish.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                        Price = md.Dish.Price,
                        IsMandatory = md.Dish.IsMenuOnly,
                    })
                    .ToList(),

                    Sections = m.MenuSections.Select(ms => new SectionDto<MenuDishListItem>
                    {
                        Name = ms.Section.Name,
                        Items = ms.Section.DishesSection.Select(ds => new MenuDishListItem
                        {
                            Id = ds.Dish.Id,
                            Name = ds.Dish.Name,
                            Description = ds.Dish.Description,
                            Picture = ds.Dish.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                            Price = ds.Dish.Price,
                            IsMandatory = ds.Dish.IsMenuOnly,
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
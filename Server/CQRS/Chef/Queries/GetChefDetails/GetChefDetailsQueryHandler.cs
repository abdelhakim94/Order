using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Constants;
using Order.Server.Persistence;
using Order.Shared.Dto.Chef;
using Order.Shared.Dto.Dish;
using Order.Shared.Security.Claims;

namespace Server.CQRS.Chef.Queries
{
    public class GetChefDetailsQueryHandler : IRequestHandler<GetChefDetailsQuery, ChefDetailsDto>
    {
        private readonly IOrderContext context;

        public GetChefDetailsQueryHandler(IOrderContext context) => this.context = context;

        public async Task<ChefDetailsDto> Handle(GetChefDetailsQuery query, CancellationToken ct)
        {
            var asAnonymous = await context.Users
                .Where(u => u.Id == query.Id && u.UserProfiles.Any(p => p.IdProfile == (int)Profile.CHEF))
                .Select(u => new
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
                    Bio = u.Bio,
                    Card = u.Cards
                        .Where(c => c.IsActive)
                        .Select(c => new
                        {
                            Name = c.Name,
                            Sections = c.CardSections
                                .OrderByDescending(s => s.Order.HasValue)
                                .ThenBy(s => s.Order)
                                .Select(cs => new
                                {
                                    Id = cs.Section.Id,
                                    Name = cs.Section.Name,
                                    Dishes = cs.Section.DishesSection
                                        .Select(ds => new
                                        {
                                            Id = ds.Dish.Id,
                                            Name = ds.Dish.Name,
                                            Picture = ds.Dish.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                                            Description = ds.Dish.Description,
                                            Price = ds.Dish.Price,
                                            Order = ds.Order,
                                            IsMenu = false,
                                        })
                                        .ToList(),
                                    Menues = cs.Section.MenuesSection
                                            .Select(ms => new
                                            {
                                                Id = ms.Menu.Id,
                                                Name = ms.Menu.Name,
                                                Picture = ms.Menu.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                                                Description = ms.Menu.Description,
                                                Price = ms.Menu.Price,
                                                Order = ms.Order,
                                                IsMenu = true,
                                            })
                                            .ToList(),
                                }).ToList(),
                        })
                        .SingleOrDefault(),
                })
                .SingleOrDefaultAsync();

            return new()
            {
                Id = asAnonymous.Id,
                Picture = asAnonymous.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                ChefFullName = asAnonymous.ChefFullName,
                City = asAnonymous.City,
                Bio = asAnonymous.Bio,
                Card = new()
                {
                    Name = asAnonymous.Card.Name,
                    Sections = asAnonymous.Card.Sections.Select(s => new SectionDto<DishOrMenuListItemDto>
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Items = s.Dishes
                            .Union(s.Menues)
                            .OrderByDescending(e => e.Order.HasValue)
                            .ThenBy(e => e.Order)
                            .Select(d => new DishOrMenuListItemDto
                            {
                                Id = d.Id,
                                Name = d.Name,
                                Picture = d.Picture ?? NoDataFallbacks.NO_DATA_IMAGE,
                                Description = d.Description,
                                Price = d.Price,
                                IsMenu = d.IsMenu,
                            })
                            .ToList(),
                    })
                    .ToList(),
                }
            };
        }
    }
}

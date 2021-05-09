using MediatR;
using Order.Server.Dto;
using Order.Shared.Dto;
using Order.Shared.Dto.Dish;

namespace Order.Server.CQRS.Dish.Queries
{
    public class SearchForMenuesQuery : IRequest<PaginatedList<MenuDetailsDto>>
    {
        public MenuesSearchFilter Filter { get; }

        public SearchForMenuesQuery(MenuesSearchFilter filter)
            => Filter = filter;
    }
}

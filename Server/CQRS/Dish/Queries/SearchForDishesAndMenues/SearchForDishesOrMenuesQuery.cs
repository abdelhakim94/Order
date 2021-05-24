using MediatR;
using Order.Server.Constants;
using Order.Server.Dto;
using Order.Shared.Dto;
using Order.Shared.Dto.Dish;

namespace Order.Server.CQRS.Dish.Queries
{
    public class SearchForDishesOrMenuesQuery : IRequest<PaginatedList<DishOrMenuListItemDto>>
    {
        public DishesOrMenuesSearchFilter Filter { get; }
        public SearchDishOrMenu DishOrMenu { get; }

        public SearchForDishesOrMenuesQuery(DishesOrMenuesSearchFilter filter, SearchDishOrMenu dishOrMenu)
        {
            Filter = filter;
            DishOrMenu = dishOrMenu;
        }
    }
}
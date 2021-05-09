using System.Collections.Generic;
using MediatR;
using Order.Server.Dto;
using Order.Shared.Dto;
using Order.Shared.Dto.Dish;

namespace Order.Server.CQRS.Dish.Queries
{
    public class SearchForDishesQuery : IRequest<PaginatedList<DishDetailsDto>>
    {
        public DishesSearchFilter Filter { get; }

        public SearchForDishesQuery(DishesSearchFilter filter)
        {
            Filter = filter;
        }
    }
}

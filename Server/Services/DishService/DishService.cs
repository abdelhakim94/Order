using System.Threading.Tasks;
using MediatR;
using Order.Server.Constants;
using Order.Server.CQRS.Dish.Queries;
using Order.Server.Dto;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Dish;

namespace Order.Server.Services
{
    public class DishService : IDishService, IScopedService
    {
        private readonly IMediator mediator;

        public DishService(IMediator mediator) => this.mediator = mediator;

        public Task<PaginatedList<DishOrMenuListItemDto>> SearchForDishes(DishesOrMenuesSearchFilter filter)
        {
            return mediator.Send(new SearchForDishesOrMenuesQuery(filter, SearchDishOrMenu.DISHES));
        }

        public Task<PaginatedList<DishOrMenuListItemDto>> SearchForMenues(DishesOrMenuesSearchFilter filter)
        {
            return mediator.Send(new SearchForDishesOrMenuesQuery(filter, SearchDishOrMenu.MENUES));
        }

        public Task<PaginatedList<DishOrMenuListItemDto>> SearchForDishesAndMenues(DishesOrMenuesSearchFilter filter)
        {
            return mediator.Send(new SearchForDishesOrMenuesQuery(filter, SearchDishOrMenu.DISHES_AND_MENUES));
        }
    }
}

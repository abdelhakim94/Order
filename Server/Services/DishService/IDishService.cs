using System.Threading.Tasks;
using Order.Server.Dto;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Dish;

namespace Order.Server.Services
{
    public interface IDishService : IScopedService
    {
        Task<PaginatedList<DishOrMenuListItemDto>> SearchForDishes(DishesOrMenuesSearchFilter filter);
        Task<PaginatedList<DishOrMenuListItemDto>> SearchForMenues(DishesOrMenuesSearchFilter filter);
        Task<PaginatedList<DishOrMenuListItemDto>> SearchForDishesAndMenues(DishesOrMenuesSearchFilter filter);
    }
}

using System.Threading.Tasks;
using Order.Server.Dto;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Dish;

namespace Order.Server.Services
{
    public interface IDishService : IScopedService
    {
        Task<PaginatedList<DishDetailsDto>> SearchForDishes(DishesSearchFilter filter);
        Task<PaginatedList<MenuDetailsDto>> SearchForMenues(MenuesSearchFilter filter);
        Task<PaginatedList<DishAndMenuDetailsDto>> SearchForDishesAndMenues(DishesAndMenuesSearchFilter filter);
    }
}

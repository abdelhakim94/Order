using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Dish;

namespace Order.Server.Services
{
    public interface IDishService : IScopedService
    {
        Task<List<DishDetailsDto>> SearchForDishes(DishesSearchFilter filter);
        Task<List<MenuDetailsDto>> SearchForMenues(MenuesSearchFilter filter);
        Task<List<DishOrMenuDetailsDto>> SearchForDishesAndMenues(DishesAndMenuesSearchFilter filter);
    }
}

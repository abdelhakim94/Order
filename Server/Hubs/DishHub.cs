using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Order.Server.Dto;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Dish;

namespace Order.Server.Hubs
{
    public partial class AppHub : Hub<IClientHubMessage>
    {
        public async Task<PaginatedList<DishOrMenuListItemDto>> GetDishes(DishesOrMenuesSearchFilter filter)
        {
            return await dishService.SearchForDishes(filter);
        }

        public async Task<PaginatedList<DishOrMenuListItemDto>> GetMenues(DishesOrMenuesSearchFilter filter)
        {
            return await dishService.SearchForMenues(filter);
        }

        public async Task<PaginatedList<DishOrMenuListItemDto>> GetDishesAndMenues(DishesOrMenuesSearchFilter filter)
        {
            return await dishService.SearchForDishesAndMenues(filter);
        }

        public async Task<DishDetailsDto> GetDishDetails(int id)
        {
            return await dishService.GetDishDetails(id);
        }

        public async Task<MenuDetailsDto> GetMenuDetails(int id)
        {
            return await dishService.GetMenuDetails(id);
        }
    }
}

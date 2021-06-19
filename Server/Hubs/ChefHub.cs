using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Order.Server.Dto;
using Order.Shared.Contracts;
using Order.Shared.Dto.Chef;

namespace Order.Server.Hubs
{
    public partial class AppHub : Hub<IClientHubMessage>
    {
        public Task<PaginatedList<ChefListItemDto>> GetChefs(ChefsSearchFilter filter)
        {
            return chefService.SearchForChefs(filter);
        }

        public Task<ChefDetailsDto> GetChefDetails(int id)
        {
            return chefService.GetChefDetails(id);
        }
    }
}

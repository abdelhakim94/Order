using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Address;
using Order.Shared.Security;

namespace Order.Server.Hubs
{
    public partial class AppHub : Hub<IClientHubMessage>
    {
        public async Task<List<IdentifiedUserAddressDetailDto>> GetAllUserAddresses()
        {
            return await userService.GetAllUserAddresses(Context.User.GetUserId().Value);
        }

        public async Task<UserAddressDetailDto> GetLastUsedAddress()
        {
            return await userService.GetLastUsedAddress(Context.User.GetUserId().Value);
        }

        public async Task<bool> SaveUserAddress(UserAddressDetailDto address)
        {
            return await userService.SaveUserAddress(address, Context.User.GetUserId().Value);
        }

        public async Task<List<DatalistOption>> SearchCities(string search)
        {
            return await userService.SearchCities(search);
        }
    }
}

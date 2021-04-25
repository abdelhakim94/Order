using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Address;

namespace Order.Server.Services
{
    public interface IUserService : IScopedService
    {
        Task<List<UserAddressDetailDto>> GetAllUserAddresses(int userId);
        Task<UserAddressDetailDto> GetLastUsedAddress(int userId);
        Task<bool> SaveUserAddress(UserAddressDetailDto address, int userId);
        Task<List<DatalistOption>> SearchCities(string search);
    }
}

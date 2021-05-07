using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Order.Server.CQRS.User.Commands;
using Order.Server.CQRS.User.Queries;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Address;

namespace Order.Server.Services
{
    public class UserService : IUserService, IScopedService
    {
        private readonly IMediator mediator;

        public UserService(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<List<IdentifiedUserAddressDetailDto>> GetAllUserAddresses(int userId)
        {
            var result = await mediator.Send(new GetAllUserAddressesQuery(userId));
            var counter = 0;
            return result?.Select(r => new IdentifiedUserAddressDetailDto
            {
                Id = (counter++).ToString(),
                Address = r,
            }).ToList();
        }

        public Task<UserAddressDetailDto> GetLastUsedAddress(int userId)
        {
            return mediator.Send(new GetLastUsedAddressQuery(userId));
        }

        public Task<bool> SaveUserAddress(UserAddressDetailDto address, int userId)
        {
            return mediator.Send(new SaveUserAddressCommand(address, userId));
        }

        public Task<List<DatalistOption>> SearchCities(string search)
        {
            return mediator.Send(new SearchCitiesQuery(search));
        }
    }
}

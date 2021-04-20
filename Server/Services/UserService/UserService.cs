using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Order.Server.CQRS.User.Commands;
using Order.Server.CQRS.User.Queries;
using Order.Shared.Contracts;
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
        public Task<List<UserAddressDetailDto>> GetAllUserAddresses(int userId)
        {
            return mediator.Send(new GetAllUserAddressesQuery(userId));
        }

        public Task<UserAddressDetailDto> GetLastUsedAddress(int userId)
        {
            return mediator.Send(new GetLastUsedAddressQuery(userId));
        }

        public Task<bool> SaveUserAddress(UserAddressDetailDto address, int userId)
        {
            return mediator.Send(new SaveUserAddressCommand(address, userId));
        }
    }
}

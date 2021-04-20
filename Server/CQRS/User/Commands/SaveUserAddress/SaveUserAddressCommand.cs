using MediatR;
using Order.Shared.Dto.Address;

namespace Order.Server.CQRS.User.Commands
{
    public class SaveUserAddressCommand : IRequest<bool>
    {
        public UserAddressDetailDto Address { get; }
        public int IdUser { get; }

        public SaveUserAddressCommand(UserAddressDetailDto address, int idUser)
        {
            Address = address;
            IdUser = idUser;
        }
    }
}

using MediatR;
using Order.Shared.Dto.Address;

namespace Order.Server.CQRS.User.Queries
{
    public class GetLastUsedAddressQuery : IRequest<UserAddressDetailDto>
    {
        public int IdUser { get; }

        public GetLastUsedAddressQuery(int idUser)
        {
            IdUser = idUser;
        }
    }
}

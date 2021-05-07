using System.Collections.Generic;
using MediatR;
using Order.Shared.Dto.Address;

namespace Order.Server.CQRS.User.Queries
{
    public class GetAllUserAddressesQuery : IRequest<List<UserAddressDetailDto>>
    {
        public int IdUser { get; }

        public GetAllUserAddressesQuery(int idUser)
        {
            IdUser = idUser;
        }
    }
}

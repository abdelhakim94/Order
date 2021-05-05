using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;
using Order.Shared.Dto.Address;

namespace Order.Server.CQRS.User.Queries
{
    public class GetAllUserAddressesQueryHandler : IRequestHandler<GetAllUserAddressesQuery, List<UserAddressDetailDto>>
    {
        private readonly IOrderContext context;

        public GetAllUserAddressesQueryHandler(IOrderContext context)
            => this.context = context;

        public async Task<List<UserAddressDetailDto>> Handle(GetAllUserAddressesQuery query, CancellationToken ct)
        {
            return await context.UserAddress
                .Where(ua => ua.IdUser == query.IdUser)
                .OrderByDescending(ua => ua.LastTimeUsed)
                .Select(ua => new UserAddressDetailDto
                {
                    Address1 = ua.Address.Address1,
                    Address2 = ua.Address.Address2,
                    IdCity = ua.Address.City.Id,
                    City = ua.Address.City.Name,
                    Wilaya = ua.Address.City.Wilaya.Name,
                })
                .ToListAsync();
        }
    }
}

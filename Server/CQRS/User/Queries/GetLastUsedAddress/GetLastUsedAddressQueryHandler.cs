using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;
using Order.Shared.Dto.Address;

namespace Order.Server.CQRS.User.Queries
{
    public class GetLastUsedAddressQueryHandler : IRequestHandler<GetLastUsedAddressQuery, UserAddressDetailDto>
    {
        private readonly IOrderContext context;

        public GetLastUsedAddressQueryHandler(IOrderContext context)
        {
            this.context = context;
        }

        public async Task<UserAddressDetailDto> Handle(GetLastUsedAddressQuery query, CancellationToken ct)
        {
            return await context.UserAddress
                .AsNoTracking()
                .Where(ua => ua.IdUser == query.IdUser)
                .OrderByDescending(ua => ua.LastTimeUsed)
                .Select(ua => new UserAddressDetailDto
                {
                    Address1 = ua.Address.Address1,
                    Address2 = ua.Address.Address2,
                    IdCity = ua.Address.City.Id,
                    City = ua.Address.City.Name,
                    Longitude = ua.Address.City.Longitude,
                    Latitude = ua.Address.City.Latitude,
                    Wilaya = ua.Address.City.Wilaya.Name,
                })
                .FirstOrDefaultAsync();
        }
    }
}

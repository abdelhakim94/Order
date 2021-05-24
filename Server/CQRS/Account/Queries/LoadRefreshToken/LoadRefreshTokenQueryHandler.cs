using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Dto.Users;
using Order.Server.Persistence;

namespace Order.Server.CQRS.Account.Queries
{
    public class LoadRefreshTokenQueryHandler : IRequestHandler<LoadRefreshTokenQuery, RefreshTokenDto>
    {
        private readonly IOrderContext context;

        public LoadRefreshTokenQueryHandler(IOrderContext context) => this.context = context;

        public Task<RefreshTokenDto> Handle(LoadRefreshTokenQuery query, CancellationToken ct)
        {
            return context.UserRefreshToken
                .AsNoTracking()
                .Select(rt => new RefreshTokenDto
                {
                    UserId = rt.UserId,
                    Token = rt.Token,
                    ExpireAt = rt.ExpireAt,
                })
                .FirstOrDefaultAsync(rt => rt.UserId == query.UserId, ct);
        }
    }
}

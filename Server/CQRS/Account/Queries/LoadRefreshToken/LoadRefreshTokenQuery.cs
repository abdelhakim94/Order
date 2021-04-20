using MediatR;
using Order.Server.Dto.Users;

namespace Order.Server.CQRS.Account.Queries
{
    public class LoadRefreshTokenQuery : IRequest<RefreshTokenDto>
    {
        public int UserId { get; set; }

        public LoadRefreshTokenQuery(int userId) => UserId = userId;
    }
}

using MediatR;
using Order.Server.Dto.Jwt;

namespace Order.Server.CQRS.User.Queries
{
    public class LoadRefreshTokenQuery : IRequest<RefreshTokenDto>
    {
        public int UserId { get; set; }

        public LoadRefreshTokenQuery(int userId) => UserId = userId;
    }
}

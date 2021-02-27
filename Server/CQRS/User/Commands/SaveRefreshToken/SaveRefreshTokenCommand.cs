using System;
using MediatR;

namespace Order.Server.CQRS.User.Commands
{
    public class SaveRefreshTokenCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireAt { get; set; }

        public SaveRefreshTokenCommand(int userId, string refreshToken, DateTime expireAt)
        {
            UserId = userId;
            RefreshToken = refreshToken;
            ExpireAt = expireAt;
        }
    }
}

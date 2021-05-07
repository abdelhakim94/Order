using MediatR;

namespace Order.Server.CQRS.Account.Commands
{
    public class DeleteRefreshTokenCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public DeleteRefreshTokenCommand(int userId) => UserId = userId;
    }
}

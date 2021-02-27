using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;

namespace Order.Server.CQRS.User.Commands
{
    public class DeleteRefreshTokenCommandHandler : IRequestHandler<DeleteRefreshTokenCommand, bool>
    {
        private readonly IOrderContext context;

        public DeleteRefreshTokenCommandHandler(IOrderContext context) => this.context = context;

        public async Task<bool> Handle(DeleteRefreshTokenCommand command, CancellationToken ct)
        {
            var token = await context.UserRefreshToken
                .FirstOrDefaultAsync(rt => rt.UserId == command.UserId);

            context.UserRefreshToken.Remove(token);

            return (await context.SaveChangesAsync(ct)) > 0;
        }
    }
}

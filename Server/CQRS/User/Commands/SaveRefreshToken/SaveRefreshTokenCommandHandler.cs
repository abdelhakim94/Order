using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.DomainModel;
using Order.Server.Persistence;

namespace Order.Server.CQRS.User.Commands
{
    public class SaveRefreshTokenCommandHandler : IRequestHandler<SaveRefreshTokenCommand, bool>
    {
        private readonly IOrderContext context;

        public SaveRefreshTokenCommandHandler(IOrderContext context) => this.context = context;

        public async Task<bool> Handle(SaveRefreshTokenCommand command, CancellationToken ct)
        {
            var existingToken = await context.UserRefreshToken
                .FirstOrDefaultAsync(rt => rt.UserId == command.UserId);

            if (existingToken is null)
            {
                context.UserRefreshToken.Add(new UserRefreshToken
                {
                    UserId = command.UserId,
                    Token = command.RefreshToken,
                    ExpireAt = command.ExpireAt,
                });
            }
            else
            {
                existingToken.Token = command.RefreshToken;
                existingToken.ExpireAt = command.ExpireAt;
            }

            return (await context.SaveChangesAsync(ct)) > 0;
        }
    }
}

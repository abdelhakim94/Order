using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;

namespace Order.Server.CQRS.Account.Commands
{
    public class DeleteRefreshTokenCommandValidator : AbstractValidator<DeleteRefreshTokenCommand>
    {
        public DeleteRefreshTokenCommandValidator(IOrderContext context)
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(cmd => cmd.UserId)
                .MustAsync((userId, ct) => context.UserRefreshToken.AnyAsync(t => t.UserId == userId, ct))
                .WithMessage((cmd, userId) => $"L'utilisateur d'ID {userId} n'existe pas");
        }
    }
}

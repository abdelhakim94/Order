using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;

namespace Order.Server.CQRS.User.Commands
{
    public class DeleteRefreshTokenCommandValidator : AbstractValidator<DeleteRefreshTokenCommand>
    {
        public DeleteRefreshTokenCommandValidator(IOrderContext context)
        {
            RuleFor(cmd => cmd.UserId)
                .MustAsync((userId, ct) => context.UserRefreshToken.AnyAsync(t => t.UserId == userId, ct))
                .WithMessage(userId => $"L'utilisateur d'ID {userId} n'existe pas");
        }
    }
}

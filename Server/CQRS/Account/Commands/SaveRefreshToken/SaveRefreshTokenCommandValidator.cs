using System;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;

namespace Order.Server.CQRS.Account.Commands
{
    public class SaveRefreshTokenCommandValidator : AbstractValidator<SaveRefreshTokenCommand>
    {
        public SaveRefreshTokenCommandValidator(IOrderContext context)
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(cmd => cmd.UserId)
                .MustAsync((userId, ct) => context.Users.AnyAsync(u => u.Id == userId, ct))
                .WithMessage((cmd, userId) => $"L'utilisateur d'ID {userId} n'existe pas");

            RuleFor(cmd => cmd.RefreshToken)
                .NotEmpty()
                .WithMessage("Le refresh token est invalide");

            RuleFor(cmd => cmd.ExpireAt)
                .Must(date => date > DateTime.Now)
                .WithMessage("Le token est déja expiré");
        }
    }
}

using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;

namespace Order.Server.CQRS.Account.Commands
{
    public class AssociateToProfileCommandValidator : AbstractValidator<AssociateToProfileCommand>
    {
        public AssociateToProfileCommandValidator(IOrderContext context)
        {
            RuleFor(cmd => cmd.UserId)
                .MustAsync((userId, ct) => context.Users.AnyAsync(u => u.Id == userId, ct))
                .WithMessage((cmd, userId) => $"L'utilisateur d'ID {userId} n'existe pas");

            RuleFor(cmd => cmd.Profile)
                .MustAsync((profile, ct) =>
                {
                    var cast = (int)profile;
                    return context.Profile.AnyAsync(p => p.Id == cast, ct);
                })
                .WithMessage((cmd, profile) => $"Le profile {profile} n'existe pas");
        }
    }
}

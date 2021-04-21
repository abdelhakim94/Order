using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.Server.Persistence;

namespace Order.Server.CQRS.User.Commands
{
    public class SaveUserAddressCommandValidator : AbstractValidator<SaveUserAddressCommand>
    {
        public SaveUserAddressCommandValidator(IOrderContext context)
        {
            RuleFor(cmd => cmd.Address.Address1)
                .NotEmpty();

            RuleFor(cmd => cmd.Address.ZipCode)
                .MustAsync((zc, ct) => context.City.AnyAsync(c => c.ZipCode == zc))
                .WithMessage((cmd, zc) => $"Le code postal '{zc}' n'éxiste pas");
        }
    }
}

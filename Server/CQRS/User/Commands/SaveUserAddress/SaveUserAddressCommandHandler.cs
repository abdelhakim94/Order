using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.DomainModel;
using Order.Server.Persistence;

namespace Order.Server.CQRS.User.Commands
{
    public class SaveUserAddressCommandHandler : IRequestHandler<SaveUserAddressCommand, bool>
    {
        private readonly IOrderContext context;

        public SaveUserAddressCommandHandler(IOrderContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(SaveUserAddressCommand command, CancellationToken ct)
        {
            var address = await context.Address
                .Include(a => a.UsersAddress)
                .FirstOrDefaultAsync(a =>
                    a.Address1 == command.Address.Address1 &&
                    a.Address2 == command.Address.Address2 &&
                    a.ZipCodeCity == command.Address.ZipCode);

            if (address is not null)
            {
                if (address.UsersAddress.Any(ua => ua.IdUser == command.IdUser))
                    return true;

                address.UsersAddress.Add(new UserAddress { IdUser = command.IdUser });
                return await context.SaveChangesAsync(ct) > 0;
            }

            var newAddress = new Address
            {
                Address1 = command.Address.Address1,
                Address2 = command.Address.Address2,
                ZipCodeCity = command.Address.ZipCode,
            };
            newAddress.UsersAddress.Add(new UserAddress { IdUser = command.IdUser });
            context.Address.Add(newAddress);
            return await context.SaveChangesAsync(ct) > 0;
        }
    }
}

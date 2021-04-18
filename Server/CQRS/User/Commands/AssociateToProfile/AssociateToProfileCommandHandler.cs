using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.DomainModel;
using Order.Server.Persistence;

namespace Order.Server.CQRS.User.Commands
{
    public class AssociateToProfileCommandHandler : IRequestHandler<AssociateToProfileCommand, bool>
    {
        private readonly IOrderContext context;

        public AssociateToProfileCommandHandler(IOrderContext context)
            => this.context = context;

        public async Task<bool> Handle(AssociateToProfileCommand command, CancellationToken ct)
        {
            // needs to cast before using in query because of bug:
            // https://github.com/npgsql/efcore.pg/issues/1281
            var profile = (int)command.Profile;

            var alreadyAssociated = await context.UserProfile
                .AnyAsync(up => up.IdUser == command.UserId && up.IdProfile == profile);

            if (alreadyAssociated)
            {
                return true;
            }

            context.UserProfile.Add(new UserProfile
            {
                IdUser = command.UserId,
                IdProfile = profile,
            });

            return await context.SaveChangesAsync(ct) > 0;
        }
    }
}

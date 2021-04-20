using MediatR;
using Order.Shared.Security.Claims;

namespace Order.Server.CQRS.Account.Commands
{
    public class AssociateToProfileCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public Profile Profile { get; set; }

        public AssociateToProfileCommand(int userId, Profile profile)
        {
            UserId = userId;
            Profile = profile;
        }
    }
}

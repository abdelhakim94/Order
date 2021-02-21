using Microsoft.EntityFrameworkCore;
using Order.DomainModel;

namespace Order.IdentityPersistence
{
    public interface IIdentityPersistenceContext
    {
        DbSet<User> Users { get; set; }
        DbSet<UserClaim> UserClaims { get; set; }
        DbSet<UserLogin> UserLogins { get; set; }
        DbSet<UserToken> UserTokens { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RoleClaim> RoleClaims { get; set; }
    }
}

using IdentityServer4.EntityFramework.Interfaces;

namespace Order.Server.Model
{
    public interface IOrderContext : IPersistedGrantDbContext, IConfigurationDbContext { }
}

using System.Threading.Tasks;
using Order.Client.Components.Misc;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public interface IHubConnectionService : ISingletonService
    {
        Task StartNew(string accessToken);
        Task ShutDown();
        Task<T> Invoke<T>(string methodName);
    }
}

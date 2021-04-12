using System.Threading.Tasks;
using Order.Client.Components.Misc;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public interface IHubConnectionService : ISingletonService
    {
        bool IsConnected { get; }
        string LastAccessToken { get; }
        Task StartNew(string accessToken);
        Task ShutDown();
        Task<T> Invoke<T>(string methodName, Toast toast = default(Toast));
    }
}

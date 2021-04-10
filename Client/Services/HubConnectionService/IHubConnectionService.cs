using System.Threading.Tasks;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public interface IHubConnectionService : ISingletonService
    {
        bool IsConnected { get; }
        string LastAccessToken { get; }
        Task StartNew(string accessToken);
        Task ShutDown();
    }
}

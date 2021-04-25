using System;
using System.Threading.Tasks;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public interface IHubConnectionService : ISingletonService
    {
        Task StartNew(string accessToken);
        Task ShutDown();
        Task<T> Invoke<T>(string methodName);
        Task<T> Invoke<T, U>(string methodName, U arg1);
    }
}

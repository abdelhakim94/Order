using System;
using System.Threading.Tasks;
using Order.Client.Components.Misc;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public interface IHubConnectionService : ISingletonService
    {
        Task StartNew(string accessToken);
        Task ShutDown();
        Task<T> Invoke<T>(string methodName, Toast toast = default(Toast));
        Task<T> Invoke<T, U>(string methodName, U arg1, Toast toast = default(Toast));
    }
}

using System.Threading.Tasks;
using Order.Client.Components.Misc;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public interface IHttpClientService : IScopedService
    {
        Task<bool> Get(string url, Toast toast = default(Toast));
        Task<T> Get<T>(string url, Toast toast = default(Toast));

        Task<bool> Post<T>(string url, T toSend, Toast toast = default(Toast));
        Task<U> Post<T, U>(string url, T toSend, Toast toast = default(Toast));
    }
}

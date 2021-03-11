using System.Threading.Tasks;
using Order.Client.Components.Misc;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public interface IHttpClientService : IService
    {
        Task<bool> Get(string url, NotificationModal notificationModal = default(NotificationModal));
        Task<T> Get<T>(string url, NotificationModal notificationModal = default(NotificationModal));

        Task<bool> Post<T>(string url, T toSend, NotificationModal notificationModal = default(NotificationModal));
        Task<U> Post<T, U>(string url, T toSend, NotificationModal notificationModal = default(NotificationModal));
    }
}

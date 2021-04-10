using Microsoft.AspNetCore.SignalR;
using Order.Shared.Contracts;

namespace Order.Server.Hubs.CategoryHub
{
    public partial class AppHub : Hub<IHubMessageHandler>
    {

    }
}

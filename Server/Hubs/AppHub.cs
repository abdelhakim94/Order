using Microsoft.AspNetCore.SignalR;
using Order.Server.Services;
using Order.Shared.Contracts;

namespace Order.Server.Hubs
{
    // Contains all the dependencies of the Hub.
    public partial class AppHub : Hub<IClientHubMessage>
    {
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;

        public AppHub(
            ICategoryService categoryService,
            IUserService userService)
        {
            this.categoryService = categoryService;
            this.userService = userService;
        }
    }
}

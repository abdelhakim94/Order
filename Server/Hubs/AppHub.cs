using Microsoft.AspNetCore.SignalR;
using Order.Server.Services;
using Order.Shared.Contracts;

namespace Order.Server.Hubs
{
    // Contains all the dependencies of the Hub.
    public partial class AppHub : Hub<IClientHubMessage>
    {
        private readonly IUserService userService;
        private readonly ICategoryService categoryService;
        private readonly IDishService dishService;
        public readonly IChefService chefService;

        public AppHub(
            IUserService userService,
            ICategoryService categoryService,
            IDishService dishService,
            IChefService chefService)
        {
            this.userService = userService;
            this.categoryService = categoryService;
            this.dishService = dishService;
            this.chefService = chefService;
        }
    }
}

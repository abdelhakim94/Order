using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Order.Server.Services;
using Order.Shared.Contracts;
using Order.Shared.Dto.Category;

namespace Order.Server.Hubs
{
    public partial class AppHub : Hub<IClientHubMessage>
    {
        public Task<List<CategoryListItemDto>> GetCategories()
        {
            return categoryService.GetCategories();
        }
    }
}

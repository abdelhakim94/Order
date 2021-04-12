using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Order.Server.Services;
using Order.Shared.Contracts;
using Order.Shared.Dto.Category;

namespace Order.Server.Hubs.CategoryHub
{
    public partial class AppHub : Hub<IClientHubMessage>
    {
        private readonly ICategoryService categoryService;

        public AppHub(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public Task<List<CategoryListItemDto>> GetCategories()
        {
            var result = categoryService.GetCategories();
            return result;
        }
    }
}

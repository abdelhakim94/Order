using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Shared.Contracts;
using Order.Shared.Dto.Category;

namespace Order.Server.Services
{
    public interface ICategoryService : IScopedService
    {
        Task<List<CategoryListItemDto>> GetCategories();
    }
}

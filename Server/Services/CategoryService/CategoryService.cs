using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Order.Server.CQRS.Category.Queries;
using Order.Shared.Contracts;
using Order.Shared.Dto.Category;

namespace Order.Server.Services
{
    public class CategoryService : ICategoryService, IScopedService
    {
        private readonly IMediator mediator;

        public CategoryService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<List<CategoryListItemDto>> GetCategories()
        {
            return mediator.Send(new GetCategoriesQuery());
        }
    }
}

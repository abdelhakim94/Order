using System.Collections.Generic;
using MediatR;
using Order.Shared.Dto.Category;

namespace Order.Server.CQRS.Category.Queries
{
    public class GetCategoriesQuery : IRequest<List<CategoryListItemDto>> { }
}

using MediatR;
using Order.Server.Dto;
using Order.Shared.Dto.Chef;

namespace Order.Server.CQRS.Chef.Queries
{
    public class SearchForChefsQuery : IRequest<PaginatedList<ChefListItemDto>>
    {
        public ChefsSearchFilter Filter { get; set; }

        public SearchForChefsQuery(ChefsSearchFilter filter)
        {
            Filter = filter;
        }
    }
}

using System.Threading.Tasks;
using MediatR;
using Order.Server.CQRS.Chef.Queries;
using Order.Server.Dto;
using Order.Shared.Contracts;
using Order.Shared.Dto.Chef;

namespace Order.Server.Services
{
    public class ChefService : IChefService, IScopedService
    {
        private IMediator mediator;

        public ChefService(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public Task<PaginatedList<ChefDetailsDto>> SearchForChefs(ChefsSearchFilter filter)
        {
            return mediator.Send(new SearchForChefsQuery(filter));
        }
    }
}

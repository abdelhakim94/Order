using System.Threading.Tasks;
using MediatR;
using Order.Server.Dto;
using Order.Shared.Contracts;
using Order.Shared.Dto.Chef;

namespace Order.Server.Services
{
    public interface IChefService : IScopedService
    {
        Task<PaginatedList<ChefListItemDto>> SearchForChefs(ChefsSearchFilter filter);
    }
}

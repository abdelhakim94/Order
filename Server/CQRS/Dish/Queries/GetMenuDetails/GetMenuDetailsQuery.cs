using MediatR;
using Order.Shared.Dto.Dish;

namespace Order.Server.CQRS.Dish.Queries
{
    public class GetMenuDetailsQuery : IRequest<MenuDetailsDto>
    {
        public int Id { get; }

        public GetMenuDetailsQuery(int id)
        {
            Id = id;
        }
    }
}

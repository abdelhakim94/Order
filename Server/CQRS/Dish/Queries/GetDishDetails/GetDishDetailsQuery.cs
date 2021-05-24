using MediatR;
using Order.Shared.Dto.Dish;

namespace Order.Server.CQRS.Dish.Queries
{
    public class GetDishDetailsQuery : IRequest<DishDetailsDto>
    {
        public int Id { get; }

        public GetDishDetailsQuery(int id)
        {
            Id = id;
        }
    }
}

using MediatR;
using Order.Shared.Dto.Chef;

namespace Server.CQRS.Chef.Queries
{
    public class GetChefDetailsQuery : IRequest<ChefDetailsDto>
    {
        public int Id { get; set; }

        public GetChefDetailsQuery(int id)
        {
            Id = id;
        }
    }
}

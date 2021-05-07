using System.Collections.Generic;
using MediatR;
using Order.Shared.Dto;

namespace Order.Server.CQRS.User.Queries
{
    public class SearchCitiesQuery : IRequest<List<DatalistOption>>
    {
        public string Search { get; }

        public SearchCitiesQuery(string search)
        {
            Search = search;
        }
    }
}

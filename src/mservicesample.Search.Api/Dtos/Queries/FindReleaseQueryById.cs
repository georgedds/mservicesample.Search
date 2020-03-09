using MediatR;
using mservicesample.Search.Api.DataAccess.Entities;

namespace mservicesample.Search.Api.Dtos.Queries
{
    public class FindReleaseQueryById : IRequest<Release>
    {
        public int id { get; set; }
    }
}

using MediatR;
using mservicesample.Search.Api.Dtos.Responses;

namespace mservicesample.Search.Api.Dtos.Queries
{
    public class FindReleaseQuery : IRequest<FindReleaseResult>
    {
        public string QueryText { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

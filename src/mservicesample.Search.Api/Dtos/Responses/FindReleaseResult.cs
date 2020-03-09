using mservicesample.Search.Api.DataAccess.Entities;

namespace mservicesample.Search.Api.Dtos.Responses
{
    public class FindReleaseResult
    {
        public SearchResult<Release> SearchResult { get; set; }
    }
}

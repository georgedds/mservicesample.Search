using System.Collections.Generic;

namespace mservicesample.Search.Api.Dtos.Responses
{
    public class SearchResult<T>
    {
        public long Total { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<T> Results { get; set; }

        public long ElapsedMilliseconds { get; set; }
    }
}

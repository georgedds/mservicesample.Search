using System;
using MediatR;
using mservicesample.Search.Api.Dtos.Responses;

namespace mservicesample.Search.Api.Dtos.Queries
{
    public class FindArtistQuery : IRequest<FindArtistResult>
    {
        public string QueryText { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

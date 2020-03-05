using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mservicesample.Search.Api.DataAccess.Repositories;
using mservicesample.Search.Api.Dtos;
using mservicesample.Search.Api.Dtos.Queries;
using mservicesample.Search.Api.Dtos.Responses;

namespace mservicesample.Search.Api.Services.Handlers
{
    public class FindArtistHandler  : IRequestHandler<FindArtistQuery, FindArtistResult>
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public FindArtistHandler(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<FindArtistResult> Handle(FindArtistQuery request, CancellationToken cancellationToken)
        {
            var searchResults = await _artistRepository.Find(request.QueryText,request.Page,request.PageSize);

            var mapping = _mapper.Map<SearchResult<ArtistDto>>(searchResults);

            return new FindArtistResult{  SearchResult = mapping };
        }
    }
}

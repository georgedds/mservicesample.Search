using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mservicesample.Search.Api.DataAccess.Repositories;
using mservicesample.Search.Api.Dtos.Queries;
using mservicesample.Search.Api.Dtos.Responses;

namespace mservicesample.Search.Api.Services.Handlers
{
    public class FindReleaseHandler: IRequestHandler<FindReleaseQuery, FindReleaseResult>
    {
        private readonly IReleaseRepository _releaseRepository;
        private readonly IMapper _mapper;

        public FindReleaseHandler(IReleaseRepository releaseRepository, IMapper mapper)
        {
            _releaseRepository = releaseRepository;
            _mapper = mapper;
        }
        
        public async Task<FindReleaseResult> Handle(FindReleaseQuery request, CancellationToken cancellationToken)
        {
            var searchResults = await _releaseRepository.Find(request.QueryText,request.Page,request.PageSize);
            
            return new FindReleaseResult{  SearchResult = searchResults };
        }
    }
}

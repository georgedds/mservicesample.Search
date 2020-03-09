using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using mservicesample.Search.Api.DataAccess.Entities;
using mservicesample.Search.Api.DataAccess.Repositories;
using mservicesample.Search.Api.Dtos.Queries;

namespace mservicesample.Search.Api.Services.Handlers
{
    public class FindReleaseByIdHandler: IRequestHandler<FindReleaseQueryById, Release>
    {
        private readonly IReleaseByIdRepository _releaseByIdRepository;
        private readonly IMapper _mapper;

        public FindReleaseByIdHandler(IReleaseByIdRepository releaseByIdRepository, IMapper mapper)
        {
            _releaseByIdRepository = releaseByIdRepository;
            _mapper = mapper;
        }
        
        public async Task<Release> Handle(FindReleaseQueryById request, CancellationToken cancellationToken)
        {
            var searchResults = await _releaseByIdRepository.Find(request.id);
            return  searchResults;
        }
    }
}

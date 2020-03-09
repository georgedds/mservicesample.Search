using System.Threading.Tasks;
using AutoMapper;
using mservicesample.Search.Api.DataAccess.ElasticSearch;
using mservicesample.Search.Api.DataAccess.Entities;
using mservicesample.Search.Api.Helpers;
using Nest;

namespace mservicesample.Search.Api.DataAccess.Repositories
{
    public class ReleaseByIdRepository : IReleaseByIdRepository
    {
        private readonly ElasticClient _elasticClient;
        private readonly IMapper _mapper;
        public ReleaseByIdRepository(ElasticClientProvider clientProvider,IMapper mapper)
        {
            _elasticClient = clientProvider.Client;
            _mapper = mapper;
        }
        public async Task<Release> Find(int id)
        {
            
            var response = await _elasticClient.GetAsync<Release>(new DocumentPath<Release>(id),g => g
                .Index(Constants.ElasticIndexes.Release));
            
            var mapping = _mapper.Map<Release>(response);
            return mapping;
        
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using mservicesample.Search.Api.DataAccess.ElasticSearch;
using mservicesample.Search.Api.DataAccess.Entities;
using mservicesample.Search.Api.Dtos.Responses;
using mservicesample.Search.Api.Helpers;
using Nest;

namespace mservicesample.Search.Api.DataAccess.Repositories
{
    public class ReleaseRepository : IReleaseRepository
    {
        private readonly ElasticClient _elasticClient;
        public ReleaseRepository(ElasticClientProvider clientProvider)
        {
            _elasticClient = clientProvider.Client;
        }
        public async Task<SearchResult<Release>> Find(string query,int page, int pageSize)
        {
            
            var response = await _elasticClient.SearchAsync<Release>(searchDescriptor => searchDescriptor
                .Index(Constants.ElasticIndexes.Release)
                .Query(queryContainerDescriptor => queryContainerDescriptor
                    .Match(c => c
                        .Field(p => p.title)
                        .Analyzer("standard")
                        //.Boost(1.1)
                        .Query(query)
                        //.Fuzziness(Fuzziness.AutoLength(3, 6))
                        //.Lenient()
                        //.FuzzyTranspositions()
                        //.MinimumShouldMatch(2)
                        //.Operator(Operator.And)
                        //.FuzzyRewrite(MultiTermQueryRewrite.TopTermsBlendedFreqs(10))
                        .Name("release_query")
                        .AutoGenerateSynonymsPhraseQuery(false)
                    ))
                .From((page - 1) * pageSize)
                .Size(pageSize));


            return new SearchResult<Release>
            {
                Total = response.Total,
                ElapsedMilliseconds = response.Took,
                Page = page,
                PageSize = pageSize,
                Results = response.Hits.Select(h =>
                {
                    h.Source.id = h.Id;
                    return h.Source;
                }).ToList()
            };
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mservicesample.Search.Api.DataAccess.ElasticSearch;
using mservicesample.Search.Api.DataAccess.Entities;
using mservicesample.Search.Api.Dtos.Responses;
using Nest;

namespace mservicesample.Search.Api.DataAccess.Repositories
{
     public class ArtistRepository : IArtistRepository
    {
        private readonly ElasticClient _elasticClient;
        public ArtistRepository(ElasticClientProvider clientProvider)
        {
            _elasticClient = clientProvider.Client;
        }
        public async Task<SearchResult<Artist>> Find(string query,int page, int pageSize)
        {
            var response = await _elasticClient.SearchAsync<Artist>(
                    s =>
                        s.Query(q =>
                                q.MultiMatch(mm =>
                                    mm.Query(query)
                                        .Fields(f => f.Fields(p => p.RealName,p => p.Name))
                                        //.Fields(p => p.Id p => p.Name,p=> p.NameVariations, p=> p.RealName))
                                        .Type(TextQueryType.BestFields)
                                        //.Fuzziness(Fuzziness.Auto)
                                )
                            )
                            //.Lenient(true)
                            .From((page - 1) * pageSize)
                            .Size(pageSize));

            return new SearchResult<Artist>
            {
                Total = response.Total,
                ElapsedMilliseconds = response.Took,
                Page = page,
                PageSize = pageSize,
                Results = response.Documents.ToList()
            };
        }

        public async Task<List<AutoCompleteResult>> AutoComplete(string queryText)
        {
            var response = await _elasticClient.SearchAsync<Artist>(sa => sa
                .Suggest(sg => sg
                    .Completion("artist-name-suggest", cmp => cmp
                        .Prefix(queryText)
                        .Fuzzy(fz => fz.Fuzziness(Fuzziness.Auto))
                        .SkipDuplicates()
                        .Field(f => f.Name))));


            var suggestionsresult = response.Suggest["artist-name-suggest"].Select(s => s.Options);
            var rs = new List<AutoCompleteResult>();

            suggestionsresult.ToList().ForEach(s =>
            {
                rs.AddRange(s.Select(opt => new AutoCompleteResult
                {
                    //Id = opt.Source.Id,
                    Suggestion = opt.Source.Name
                }));
            });

            return rs;
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using mservicesample.Search.Api.DataAccess.Entities;
using mservicesample.Search.Api.Dtos.Responses;


namespace mservicesample.Search.Api.DataAccess.Repositories
{
    public interface IArtistRepository
    {
        Task<SearchResult<Artist>> Find(string queryText,int page, int pageSize);

        Task<List<AutoCompleteResult>> AutoComplete(string queryText);
    }
}

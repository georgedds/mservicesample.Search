using System.Threading.Tasks;
using mservicesample.Search.Api.DataAccess.Entities;
using mservicesample.Search.Api.Dtos.Responses;

namespace mservicesample.Search.Api.DataAccess.Repositories
{
    public interface IReleaseRepository
    {
        Task<SearchResult<Release>> Find(string queryText,int page, int pageSize);
    }
}

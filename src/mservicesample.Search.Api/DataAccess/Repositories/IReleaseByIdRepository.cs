using System.Threading.Tasks;
using mservicesample.Search.Api.DataAccess.Entities;

namespace mservicesample.Search.Api.DataAccess.Repositories
{
    public interface IReleaseByIdRepository
    {
        Task<Release> Find(int id);
    }
}

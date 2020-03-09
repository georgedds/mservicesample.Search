using Microsoft.Extensions.Options;
using Nest;

namespace mservicesample.Search.Api.DataAccess.ElasticSearch
{
    public class ElasticClientProvider
    {
        public ElasticClientProvider(IOptions<ElasticConnectionSettings> settings)
        {
            var connectionSettings = new ConnectionSettings(new System.Uri(settings.Value.ClusterUrl));
            
            connectionSettings.EnableDebugMode();

            if (settings.Value.DefaultIndex != null)
            {
                connectionSettings.DefaultIndex(settings.Value.DefaultIndex);
            }

            this.Client = new ElasticClient(connectionSettings);
        }

        public ElasticClient Client { get; }
    }
}

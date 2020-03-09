using System.Collections.Generic;

namespace mservicesample.Search.Api.DataAccess.Entities
{
    public class Artist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Profile { get; set; }
        public List<string> namevariations { get; set; }
    }
}

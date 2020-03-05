using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mservicesample.Search.Api.DataAccess.Entities
{
    public class Release
    {
        public string id{get;set;}
        public List<Artist> artists { get; set; }
        public string title { get; set; }
        public List<Label> labels { get; set; }
        public List<string> genres { get; set; }
        public List<string> styles { get; set; }
        public string country { get; set; }
        public string notes { get; set; }
        public string data_quality { get; set; }
        public List<Tracklist> tracklist { get; set; }
        public List<Video> videos { get; set; }
        public string released { get; set; }
        public List<Images> images {get;set;}

        public class Images
        {
            public string url{get;set;}
        }


        //public class Artist2
        //{
        //    public List<Artist> artist { get; set; }
        //}

        public class Artist
        {
            public List<string> id { get; set; }
            public List<string> name { get; set; }
            public List<string> anv { get; set; }
            public List<string> join { get; set; }
            public List<string> role { get; set; }
            public List<string> tracks { get; set; }
        }

   

        public class Label
        {
            public string catno { get; set; }
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Extraartist
        {
            public List<Artist> artist { get; set; }
        }

        public class Tracklist
        {
            public List<string> position { get; set; }
            public List<string> title { get; set; }
            public List<string> duration { get; set; }
            public List<Extraartist> extraartists { get; set; }
        }

        public class Video
        {
            public string duration { get; set; }
            public string embed { get; set; }
            public string src { get; set; }
        }
    }
}

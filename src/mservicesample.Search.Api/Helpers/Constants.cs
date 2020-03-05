namespace mservicesample.Search.Api.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "Admnin", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }

        public static class ElasticIndexes
        {
            public const string Artists = "artist";
            public const string Release = "release";
        }
    }
}

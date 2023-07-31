namespace Common.Options
{
    public class Urls
    {
        public required string View { get; init; }
        public required string Apigateway { get; init; }
        public required string Identity { get; init; }
        public required string Catalog { get; init; }
        public required string Cart { get; init; }
        public required string Orders { get; init; }
        public required Grpc Grpc { get; init; }
    }

    public class Grpc
    {
        public required string View { get; init; }
        public required string Apigateway { get; init; }
        public required string Identity { get; init; }
        public required string Catalog { get; init; }
        public required string Cart { get; init; }
        public required string Orders { get; init; }
    }
}

namespace MicroserviceFirst.API.Redis
{
    public class RedisOption
    {
        public required string Host { get; set; }
        public required string Port { get; set; }
        public string? Password { get; set; }
        public string ServiceName { get; set; } = "mymaster";
        public bool IsCluster { get; set; }
        public string? AppParametersCacheKey { get; set; }
    }
}
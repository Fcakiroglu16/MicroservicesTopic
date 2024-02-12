namespace MicroserviceFirst.API.Models
{
    public class SqlServerConfigurationSource : IConfigurationSource
    {
        public string? ConnectionString;
        public IConfigurationProvider Build(IConfigurationBuilder builder) => new SqlServerConfigurationProvider(this);
    }
}
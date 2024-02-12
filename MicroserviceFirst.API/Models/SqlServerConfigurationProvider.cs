using Microsoft.EntityFrameworkCore;

namespace MicroserviceFirst.API.Models
{
    public class SqlServerConfigurationProvider(SqlServerConfigurationSource source) : ConfigurationProvider
    {
        public override void Load()
        {
            var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(source.ConnectionString).Options);
            Data = context.AppSettings.ToDictionary(c => c.Key, c => c.Value)!;

            base.Load();
        }
    }
}
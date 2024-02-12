using Microsoft.EntityFrameworkCore;

namespace MicroserviceFirst.API.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<AppSettings> AppSettings { get; set; } = default!;
    }
}
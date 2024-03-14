using Microsoft.EntityFrameworkCore;

namespace MicroserviceFirst.API.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(x => x.Id).ValueGeneratedNever();
        base.OnModelCreating(modelBuilder);
    }
}
namespace MicroserviceFirst.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public int Stock { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}
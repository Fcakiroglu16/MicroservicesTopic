namespace MicroserviceFirst.API.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public List<Product> Products { get; set; } = new();
}
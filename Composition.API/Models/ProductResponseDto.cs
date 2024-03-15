namespace Composition.API.Models
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
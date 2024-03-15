namespace Composition.API.Models
{
    public class ProductCompositeResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public decimal ProductPrice { get; set; }
        public int? StockCount { get; set; }
        public string? StockCode { get; set; }
        public string? StockType { get; set; }
    }
}
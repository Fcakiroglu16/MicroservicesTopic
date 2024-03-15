namespace Composition.API.Models
{
    public class StockResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string Code { get; set; } = default!;
        public string Type { get; set; } = default!;
    }
}
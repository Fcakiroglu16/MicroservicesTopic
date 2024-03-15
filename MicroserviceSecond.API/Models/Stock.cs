namespace MicroserviceSecond.API.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string Code { get; set; } = default!;
        public string Type { get; set; } = default!;
    }
}
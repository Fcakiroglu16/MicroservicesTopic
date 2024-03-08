namespace MicroserviceFirst.API.Products.ProductStream.Events
{
    public class ProductCreatedEvent

    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }

    public record ProductNameUpdatedEvent(string Id, string Name) : EventBase;

    public record ProductStockUpdatedEvent(string Id, int Stock) : EventBase;

    public record ProductDeletedEvent(string Id) : EventBase;
}
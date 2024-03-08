using System.Text.Json.Serialization;

namespace MicroserviceFirst.API.Products.ProductStream.Events
{
    [JsonDerivedType(typeof(ProductCreatedEvent))]
    public record EventBase
    {
    }
}
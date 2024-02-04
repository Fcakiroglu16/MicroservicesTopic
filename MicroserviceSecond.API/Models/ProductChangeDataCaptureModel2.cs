using System.Text.Json.Serialization;

namespace MicroserviceSecond.API.Models
{
    public class ProductChangeDataCaptureModel2
    {
        public Payload? Payload { get; set; }
    }

    public class Payload
    {
        public Product? After { get; set; }

        public string Op { get; set; } = default!;
    }


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Price { get; set; } = default!;
    }
}
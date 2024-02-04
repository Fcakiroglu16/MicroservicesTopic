using System.Globalization;

namespace MicroserviceSecond.API.Models
{
    public class ProductChangeDataCaptureModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
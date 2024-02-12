using System.ComponentModel.DataAnnotations;

namespace MicroserviceFirst.API.Models
{
    public class AppSettings
    {
        [Key] [StringLength(100)] public string Key { get; set; } = default!;
        [StringLength(500)] public string Value { get; set; } = default!;
    }
}
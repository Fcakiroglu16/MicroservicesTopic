using Composition.API.Models;

namespace Composition.API.Services
{
    public class ProductServices(HttpClient http)
    {
        public async Task<ProductResponseDto?> Get(int id)
        {
            var response= await http.GetAsync($"/api/products/{id}");

            //if
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ProductResponseDto>() : null;
        }
    }
}
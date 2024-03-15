using Composition.API.Models;
using static System.Net.WebRequestMethods;

namespace Composition.API.Services
{
    public class StockServices(HttpClient http)
    {
        public async Task<StockResponseDto?> Get(int id)
        {
            var response = await http.GetAsync($"/api/stocks/{id}");


            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<StockResponseDto>() : null;
        }
    }
}
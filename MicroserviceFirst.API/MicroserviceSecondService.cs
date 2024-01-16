namespace MicroserviceFirst.API;

public class MicroserviceSecondService
{
    private readonly HttpClient _client;

    public MicroserviceSecondService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<string>> GetProducts()
    {
        var response = await _client.GetAsync("/api/products");

        var responseAsJson = await response.Content.ReadFromJsonAsync<List<string>>();

        return responseAsJson;
    }
}
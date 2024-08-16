using System.Net;
using System.Text.Json;
using Microsoft.OpenApi.Writers;
using Polly;

namespace MicroserviceFirst.API;

public class MicroserviceSecondService(HttpClient client)
{
    public async Task<List<string>> GetProducts()
    {
        var fallbackPolicy = Policy<HttpResponseMessage>
            .Handle<Exception>() // Tüm hataları yakalar
            .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new List<string> { "falback1", "falback2" }))
            }, async (outcome, context) =>
            {
                // Fallback sırasında yapılacak işlemler (loglama, uyarı, vb.)
                Console.WriteLine("Fallback executed due to: " + outcome.Exception?.Message ??
                                  outcome.Result.ReasonPhrase);
                await Task.CompletedTask;
            });


        var response = await fallbackPolicy.ExecuteAsync(async () => await client.GetAsync("/api/products"));


        var responseAsJson = await response.Content.ReadFromJsonAsync<List<string>>();

        return responseAsJson;
    }


    public async Task<List<string>> GetProductsWithBulkHeadPattern()
    {
        var bulkheadPolicy = Policy.BulkheadAsync(1, 1, onBulkheadRejectedAsync: (context) =>
        {
            // Bulkhead rejected olduğunda yapılacak işlemler (loglama, uyarı, vb.)
            Console.WriteLine("Bulkhead rejected");
            return Task.CompletedTask;
        });

        var response = await bulkheadPolicy.ExecuteAsync(async () => await client.GetAsync("/api/products"));

        var responseAsJson = await response.Content.ReadFromJsonAsync<List<string>>();

        return responseAsJson;
    }

    public async Task<List<string>> BulkHeadExample()
    {
        var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(maxParallelization: 2, maxQueuingActions: 4,
            onBulkheadRejectedAsync: async context =>
            {
                Console.WriteLine("Request was rejected due to bulkhead constraints.");
                await Task.CompletedTask;
            });


        using var httpClient = new HttpClient();

        // Paralel olarak birkaç istek gönderelim
        var tasks = new Task<HttpResponseMessage>[6];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = MakeRequest(httpClient, bulkheadPolicy, i + 1);
        }

        await Task.WhenAll(tasks);
    }

    static async Task<HttpResponseMessage> MakeRequest(HttpClient httpClient,
        IAsyncPolicy<HttpResponseMessage> bulkheadPolicy, int requestId)
    {
        return await bulkheadPolicy.ExecuteAsync(async () =>
        {
            Console.WriteLine($"Executing request {requestId}");
            return await httpClient.GetAsync("https://httpbin.org/delay/1");
        });
    }
}
## Bulkhead Pattern Nedir?

**Bulkhead pattern**, yazılım sistemlerinde izolasyon ve dayanıklılık sağlamak için kullanılan bir tasarım desenidir. Adını gemicilikten alır; gemilerde bulkhead'ler (bölmeler), suyun geminin bir kısmına sızmasını önleyerek geminin batmasını engeller. Aynı mantıkla, yazılımda bulkhead pattern, bir sistemdeki hataların yayılmasını engelleyerek sistemin geri kalanının sağlıklı çalışmasını sağlar.

## Bulkhead Pattern'in Faydaları
- **İzolasyon:** Bir hizmet veya bileşen başarısız olduğunda, bu hatanın sistemin geri kalanını etkilemesini önler.
- **Dayanıklılık:** Kritik hizmetlerin, daha az kritik olanlar nedeniyle çökmesini engeller. Örneğin, bir e-ticaret sitesinde ödeme hizmeti başarısız olursa, diğer hizmetlerin (ürün arama gibi) çalışmaya devam etmesini sağlar.
- **Kapasite Yönetimi:** Belirli kaynaklar için ayrılmış kapasite sağlar. Örneğin, bir sistemde belirli bir sayıda iş parçacığı sadece kritik işlemler için ayrılabilir.

## Bulkhead Pattern Kullanımı

Aşağıda, bir örnek ASP.NET Core uygulamasında Bulkhead Pattern kullanımını gösteren bir örnek bulunmaktadır.

```csharp
using Polly;
using Polly.Bulkhead;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Bulkhead politikası oluşturma
        var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(maxParallelization: 2, maxQueuingActions: 4, onBulkheadRejectedAsync: async context =>
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

        Console.WriteLine("All requests completed.");
    }

    static async Task<HttpResponseMessage> MakeRequest(HttpClient httpClient, IAsyncPolicy<HttpResponseMessage> bulkheadPolicy, int requestId)
    {
        return await bulkheadPolicy.ExecuteAsync(async () =>
        {
            Console.WriteLine($"Executing request {requestId}");
            return await httpClient.GetAsync("https://httpbin.org/delay/1");
        });
    }
}```



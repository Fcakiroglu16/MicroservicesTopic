## Polly ile Rate Limiter Kullanımı

**Polly**, .NET uygulamalarında esneklik ve dayanıklılık sağlamak için kullanılan popüler bir kütüphanedir. Polly, hata yönetimi, yeniden deneme (retry), circuit breaker, timeout ve daha birçok senaryo için politikalar tanımlamanıza olanak tanır. **Rate Limiter** politikası da bunlardan biridir ve belirli bir süre içinde yapılan istek sayısını sınırlamak için kullanılır.

### Rate Limiter'in Faydaları
- **Aşırı Yüklenmeyi Önleme:** Bir hizmetin veya sistemin aşırı istek yükü altında kalmasını engeller.
- **Kaynakların Korunması:** Sınırlı kaynakları korur ve adil bir kullanım sağlar.
- **Hizmet Kalitesini Artırma:** Ani trafik artışları durumunda bile hizmet kalitesini korumak için kullanılır.
- **Kötüye Kullanımı Önleme:** Kötü niyetli saldırılara karşı bir savunma mekanizması olarak işlev görür.

## Polly ile Rate Limiter Örneği

Aşağıda, Polly kütüphanesi kullanarak bir Rate Limiter politikası oluşturmanın ve uygulamanın nasıl yapılacağını gösteren bir örnek bulunmaktadır.

```csharp
using Polly;
using Polly.RateLimit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Polly Rate Limiter politikası
        var rateLimiterPolicy = Policy.RateLimitAsync(10, TimeSpan.FromSeconds(30)); 
        // 30 saniye içinde en fazla 10 istek

        using var httpClient = new HttpClient();

        for (int i = 0; i < 15; i++)
        {
            try
            {
                // Rate Limiter politikasını uygula
                await rateLimiterPolicy.ExecuteAsync(async () =>
                {
                    var response = await httpClient.GetAsync("https://httpbin.org/get");
                    Console.WriteLine($"Request {i + 1}: {response.StatusCode}");
                });
            }
            catch (RateLimitRejectedException ex)
            {
                Console.WriteLine($"Request {i + 1} rejected: {ex.Message}");
            }
        }
    }
}

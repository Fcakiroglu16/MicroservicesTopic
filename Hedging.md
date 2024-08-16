## Polly ile Hedging Policy Kullanımı

**Hedging**, bir isteğin başarısız olma olasılığını azaltmak için birden fazla paralel deneme yapma stratejisidir. Bu, özellikle yüksek gecikme süreli veya arızalanma riski yüksek olan hizmetlerle çalışırken kullanışlıdır. Hedging, Polly kütüphanesi ile birlikte kullanılarak, birden fazla isteğin aynı anda gönderilmesini sağlar ve ilk başarılı olan sonuç döndürülür. Bu sayede, isteklerin başarı oranı artırılır ve daha hızlı yanıt alınabilir.

### Hedging Policy'nin Faydaları
- **Yanıt Süresini Azaltma:** Paralel olarak birden fazla istek göndererek en hızlı yanıt veren isteği seçer.
- **Başarı Oranını Artırma:** Arızalanma riski yüksek olan hizmetlerde, başarılı bir yanıt alma olasılığını artırır.
- **Kritik Uygulamalar için Dayanıklılık:** Özellikle kritik işlemler için daha güvenilir bir çözüm sunar.





hedgeAction, Polly'deki Hedging Policy'nin bir parçası olarak kullanılan bir eylemdir. Bu eylem, bir hedging denemesi (hedged attempt) başlatıldığında hangi işlemin yapılacağını belirler. Hedging, aynı işlemin paralel olarak birden fazla kez başlatılmasını sağlar, böylece ilk başarılı olan işlemin sonucu alınır ve daha hızlı bir yanıt elde edilir.

hedgeAction Ne İşe Yarar?
Hedging Denemeleri: hedgeAction, hedging sırasında yapılacak olan işlemi tanımlar. Yani, ilk istek belirli bir süre içinde yanıt vermezse, Polly belirtilen hedgeAction ile bir veya daha fazla paralel istek başlatır.
Alternatif Yürütme: hedgeAction, orijinal isteğin başarısız olma olasılığına karşı bir güvenlik önlemi olarak kullanılır. Eğer ilk deneme başarılı olmazsa veya belirli bir süre içinde yanıt vermezse, hedgeAction devreye girer ve alternatif bir istek gönderilir.
İlk Başarılı İsteği Döndürme: Polly, birden fazla hedging denemesi başlattığında, ilk başarılı olan denemenin sonucunu döndürür. Diğer tüm denemeler iptal edilir.
## Polly ile Hedging Policy Örneği

Aşağıda, Polly kütüphanesi kullanarak bir Hedging Policy oluşturmanın ve uygulamanın nasıl yapılacağını gösteren bir örnek bulunmaktadır.

```csharp
using Polly;
using Polly.Hedging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Hedging Policy oluşturuyoruz
        var hedgingPolicy = Policy
            .Handle<HttpRequestException>() // HTTP isteği hatalarını yönetir
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode) // Başarısız HTTP yanıtlarını yönetir
            .HedgeAsync(
                hedgeAction: async context =>
                {
                    using var httpClient = new HttpClient();
                    return await httpClient.GetAsync("https://httpbin.org/get");
                },
                hedgingDelay: TimeSpan.FromMilliseconds(200), // İkinci istek 200ms gecikmeyle başlatılır
                maxHedgedAttempts: 3 // Maksimum 3 paralel deneme
            );

        try
        {
            var result = await hedgingPolicy.ExecuteAsync(async context =>
            {
                using var httpClient = new HttpClient();
                return await httpClient.GetAsync("https://httpbin.org/status/500"); // Başarısız olma olasılığı yüksek bir istek
            });

            Console.WriteLine($"Final response: {result.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"All hedged attempts failed: {ex.Message}");
        }
    }
}

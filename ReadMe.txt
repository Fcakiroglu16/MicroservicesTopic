Article
https://www.milanjovanovic.tech/blog/api-versioning-in-aspnetcore



`options.ReportApiVersions

`options.ReportApiVersions` parametresi, API sürümleme yapılandırmasında kullanılır ve API'nin desteklediği ve kullanımdan kaldırılan sürümlerini istemciye bildirmek için kullanılır. Bu parametre `true` olarak ayarlandığında, API yanıtlarının üstbilgilerine (headers) `api-supported-versions` ve `api-deprecated-versions` gibi bilgiler eklenir. Bu sayede istemciler, API'nin hangi sürümlerini desteklediğini ve hangi sürümlerin kullanımdan kaldırıldığını öğrenebilirler.

Bu, istemcilerin API sürümleri hakkında bilgi sahibi olmasını sağlar ve uygun sürümü kullanmalarına yardımcı olur. Örneğin, bir istemci belirli bir API sürümünü kullanıyorsa ve bu sürümün kullanımdan kaldırıldığını öğrenirse, daha yeni bir sürüme geçiş yapabilir.

Örnek:
Eğer `options.ReportApiVersions` `true` olarak ayarlanmışsa, bir API yanıtı şu şekilde görünebilir:

HTTP/1.1 200 OK
api-supported-versions: 1.0, 2.0
api-deprecated-versions: 0.9
Content-Type: application/json
...

Bu örnekte, API'nin 1.0 ve 2.0 sürümlerini desteklediği ve 0.9 sürümünün kullanımdan kaldırıldığı bilgisi istemciye iletilir.


------------------------


UrlSegmentApiVersionReader

UrlSegmentApiVersionReader sınıfı, API sürümünü URL segmentlerinden okuyan bir sürüm okuyucusudur. Bu, API sürümlemesinde kullanılan bir tekniktir ve sürüm bilgisi URL'nin bir parçası olarak belirtilir. Örneğin, /api/v1/resource gibi bir URL'de v1 kısmı API sürümünü belirtir.





AddApiExplorer

`AddApiExplorer` metodu, API sürümleme yapılandırmasında kullanılır ve API sürümlerinin Swagger/OpenAPI belgelerinde nasıl görüneceğini belirler. Bu metod, API sürümlerini gruplamak ve URL'lerde sürüm bilgilerini değiştirmek için çeşitli seçenekler sunar.

### Kullanım
`AddApiExplorer` metodu, API sürümlerini Swagger/OpenAPI belgelerinde gruplamak ve sürüm bilgilerini URL'lerde değiştirmek için kullanılır. Bu, API belgelerinin daha düzenli ve anlaşılır olmasını sağlar.

### Örnek
Aşağıdaki örnekte, `AddApiExplorer` metodu kullanılarak API sürümleri gruplandırılır ve URL'lerde sürüm bilgileri değiştirilir:


Different Types of API Versioning
The most common ways to implement API versioning are:


URL versioning: https://localhost:5001/api/v1/workouts
Header versioning: https://localhost:5001/api/workouts -H 'X-Api-Version: 1'
Query parameter versioning: https://localhost:5001/api/workouts?api-version=1






https://localhost:7092/api/Products/1 -H 'X-Api-Version: 1'
https://localhost:7092/api/Products/1?api-version=2
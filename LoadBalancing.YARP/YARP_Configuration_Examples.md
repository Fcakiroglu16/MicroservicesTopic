
# YARP Configuration: Match and Transforms Examples

## 1. Match (Path Matching)

`Match` bölümü, YARP'ın hangi istekleri yönlendireceğini belirler. `Path` anahtarı, gelen isteklerin URL yollarına bakarak bu yolları eşleştirir. Bu, proxy'nin istekleri nasıl işleyeceğini tanımlar.

### Örnek 1: Path Matching

```json
{
  "Routes": {
    "api-route": {
      "ClusterId": "microservice_one",
      "Match": {
        "Path": "/api/v1/{**catch-all}"
      }
    }
  }
}
```

Bu örnekte:
- `Path`: `/api/v1/{**catch-all}` şeklinde tanımlanmıştır.
- Bu yapı, `http://localhost/api/v1/` ile başlayan her isteği yakalar ve bu isteği proxy üzerinden yönlendirir.
- `{**catch-all}` ifadesi, yolu dinamik hale getirir ve istek yolunun devamını proxy'ye yönlendirir.

**Kullanım senaryosu**: 
- `/api/v1/products`, `/api/v1/orders`, vb. yollar bu rota ile eşleşir ve YARP bu istekleri arka uç sunucularına iletir.

### Farklı Path Matching Örneği

```json
{
  "Routes": {
    "api-route": {
      "ClusterId": "microservice_two",
      "Match": {
        "Path": "/users/{id:int}"
      }
    }
  }
}
```

Bu örnekte:
- `Path`: `/users/{id:int}` ile belirlenmiştir. Bu yapı, yalnızca `id` parametresi **tamsayı** olan istekleri yakalar.
- Örneğin, `/users/123` bu rotaya yönlendirilir, ancak `/users/john` yönlendirilmez.

---

## 2. Transforms (Path Transforms)

`Transforms`, gelen istekler üzerinde değişiklik yapmanıza olanak tanır. Bu sayede, istekleri arka uç sunucusuna yönlendirmeden önce URL yolu veya başlık bilgileri değiştirilebilir.

### Örnek 1: Path Transformation

```json
{
  "Routes": {
    "api-route": {
      "ClusterId": "microservice_one",
      "Match": {
        "Path": "/microservice-one/{**catch-all}"
      },
      "Transforms": [
        { "PathPattern": "/new-service/{**catch-all}" }
      ]
    }
  }
}
```

Bu örnekte:
- Gelen istek `/microservice-one/{**catch-all}` yoluna eşleştiğinde, YARP bu isteği `/new-service/{**catch-all}` olarak arka uç sunucusuna iletir.
- Örneğin, gelen istek `/microservice-one/products` ise YARP bunu `/new-service/products` olarak arka uca yönlendirir.

### Farklı Path Transform Örneği

```json
{
  "Routes": {
    "api-route": {
      "ClusterId": "microservice_three",
      "Match": {
        "Path": "/legacy/{**catch-all}"
      },
      "Transforms": [
        { "PathPattern": "/api/v2/{**catch-all}" }
      ]
    }
  }
}
```

Bu örnekte:
- Gelen istek `/legacy/{**catch-all}` olduğunda, YARP bunu `/api/v2/{**catch-all}` olarak değiştirir ve arka uç sunucusuna yönlendirir.
- Örneğin, `/legacy/orders/123` isteği arka uç sunucusuna `/api/v2/orders/123` olarak iletilir.

---

## Header Transformation

Başlıklarla ilgili dönüşümler de yapılabilir. Örneğin, gelen isteklere özel başlıklar eklemek istiyorsanız:

```json
{
  "Routes": {
    "api-route": {
      "ClusterId": "microservice_four",
      "Match": {
        "Path": "/api/v1/{**catch-all}"
      },
      "Transforms": [
        { "RequestHeader": "X-Added-Header", "Set": "MyHeaderValue" }
      ]
    }
  }
}
```

Bu örnekte:
- Gelen istek `/api/v1/` yoluyla eşleştiğinde, YARP isteğe `X-Added-Header: MyHeaderValue` başlığını ekler.
- Bu şekilde arka uç sunucusuna gönderilen her istekte bu başlık bulunur.

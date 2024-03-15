using Composition.API.Models;

namespace Composition.API.Services
{
    public class CompositeService(ProductServices productServices, StockServices stockServices)
    {
        public async Task<ProductCompositeResponseDto?> GetProductWithFull(int id)
        {
            var productResponse = productServices.Get(id);

            var stockResponse = stockServices.Get(id);

            await Task.WhenAll(productResponse, stockResponse);


            var product = await productResponse;
            var stock = await stockResponse;

            if (product is null) return null;


            // create productCompositionResponseDto
            var productCompositionResponseDto = new ProductCompositeResponseDto
            {
                ProductId = product!.Id,
                ProductName = product.Name,
                ProductPrice = product.Price,
            };


            if (stock is not null)
            {
                productCompositionResponseDto.StockCount = stock.Count;
                productCompositionResponseDto.StockCode = stock.Code;
                productCompositionResponseDto.StockType = stock.Type;
            }

            return productCompositionResponseDto;
        }
    }
}
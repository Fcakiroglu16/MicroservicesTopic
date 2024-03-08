using MediatR;
using MicroserviceFirst.API.DTOs;
using MicroserviceFirst.API.Products.ProductStream;
using MicroserviceFirst.API.Products.ProductStream.Events;
using MicroserviceFirst.API.ProductUseCases.ProductCreate;

namespace MicroserviceFirst.API.Products.ProductCreate
{
    public class ProductCreateCommandHandler(KafkaProductStream productStream)
        : IRequestHandler<ProductCreateCommand, ResponseDto<string>>
    {
        public async Task<ResponseDto<string>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var newProductId = Guid.NewGuid().ToString();
            await productStream.Publish(newProductId, new ProductCreatedEvent
            {
                Id = newProductId,
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = request.CategoryId
            });


            return ResponseDto<string>.SuccessWithOk(newProductId);
        }
    }
}
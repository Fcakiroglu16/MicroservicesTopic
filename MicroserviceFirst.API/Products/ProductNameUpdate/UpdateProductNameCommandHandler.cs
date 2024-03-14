using MediatR;
using MicroserviceFirst.API.DTOs;
using MicroserviceFirst.API.Products.ProductStream;
using SchemaRegistryExamples.Avro;

namespace MicroserviceFirst.API.Products.ProductNameUpdate;

public class UpdateProductNameCommandHandler(KafkaProductStream productStream)
    : IRequestHandler<UpdateProductNameCommand, ResponseDto<NoData>>
{
    public async Task<ResponseDto<NoData>> Handle(UpdateProductNameCommand request,
        CancellationToken cancellationToken)
    {
        await productStream.Publish(request.Id, new ProductNameUpdatedEvent
        {
            Id = request.Id,
            Name = request.Name
        });


        return ResponseDto<NoData>.SuccessWithNoContent();
    }
}
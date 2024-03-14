using MediatR;
using MicroserviceFirst.API.DTOs;

namespace MicroserviceFirst.API.Products.ProductNameUpdate;

public record UpdateProductNameCommand(string Id, string Name) : IRequest<ResponseDto<NoData>>;
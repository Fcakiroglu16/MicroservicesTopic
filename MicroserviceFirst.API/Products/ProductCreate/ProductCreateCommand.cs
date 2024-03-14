using MediatR;
using MicroserviceFirst.API.DTOs;

namespace MicroserviceFirst.API.Products.ProductCreate;

public record ProductCreateCommand(string Name, double Price, int Stock, int CategoryId)
    : IRequest<ResponseDto<string>>;
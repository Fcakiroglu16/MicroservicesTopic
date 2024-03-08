using MediatR;
using MicroserviceFirst.API.DTOs;

namespace MicroserviceFirst.API.ProductUseCases.ProductCreate
{
    public record ProductCreateCommand(string Name, decimal Price, int Stock, int CategoryId)
        : IRequest<ResponseDto<string>>;
}
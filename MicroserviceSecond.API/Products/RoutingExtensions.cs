namespace MicroserviceSecond.API.Products;

public static class RoutingExtensions
{
	public static RouteGroupBuilder MapProductApi(this RouteGroupBuilder group)
	{
		group.MapGet("/", () => Results.Ok(new List<string>(){"Pen 1","Pen 2","Pen 3"}));
		group.MapGet("/{id:int}", (int id) => Results.Ok($"product with id({id})"));
		group.MapPost("/", (ProductCreateRequestDto request) => 
		Results.Created(string.Empty, $"created product with name({request.Name})"));
		group.MapPut("/", (ProductUpdateRequestDto request) => Results.NoContent());
		group.MapDelete("/{id:int}", (int id) => Results.NoContent());

		return group;
	}
}
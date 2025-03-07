namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);
public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/backet{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(userName));

            return Results.Ok(result.Adapt<DeleteBasketResponse>());
        }).WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Basket Summary")
            .WithDescription("Delete Basket Description"); ; ;
    }
}

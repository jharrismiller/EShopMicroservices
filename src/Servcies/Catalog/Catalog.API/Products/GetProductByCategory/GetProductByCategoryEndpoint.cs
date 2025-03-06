
namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndpoint : ICarterModule

    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {

                var results = await sender.Send(new GetProductByCategoryQuery(category));

                return Results.Ok(new GetProductByCategoryResponse(results.Products));

            });
        }
    }



}

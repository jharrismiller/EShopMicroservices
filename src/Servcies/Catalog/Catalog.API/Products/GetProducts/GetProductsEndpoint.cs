﻿using BuildingBlocks.CQRS;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.GetProducts;


public record GetProductsRequest(int? PageNumber, int? PageSize);

public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>();
            var result = await sender.Send(query);

            var response = result.Adapt<GetProductsResponse>();

            return Results.Json(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get All Products");

    }
}

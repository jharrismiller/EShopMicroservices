﻿namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery()
    : IQuery<GetProductsQueryResult>;

public record GetProductsQueryResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession documentSession, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsQueryResult>
{
    public async Task<GetProductsQueryResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {

        logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);

        var products = await documentSession.Query<Product>().ToListAsync(cancellationToken);


        return new GetProductsQueryResult(products);
    }
}

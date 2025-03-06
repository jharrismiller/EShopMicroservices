﻿namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery()
    : IQuery<GetProductsQueryResult>;

public record GetProductsQueryResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductsQuery, GetProductsQueryResult>
{
    public async Task<GetProductsQueryResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {

        var products = await documentSession.Query<Product>().ToListAsync(cancellationToken);

        return new GetProductsQueryResult(products);
    }
}

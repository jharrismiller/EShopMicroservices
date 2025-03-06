using JasperFx.Core;

namespace Catalog.API.Products.GetProductByCategory
{

    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryQueryResult>;


    public record GetProductByCategoryQueryResult(IEnumerable<Product> Products);

    internal class GetProductByCategoryQueryHandler(IDocumentSession documentSession, ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryQueryResult>
    {
        public async Task<GetProductByCategoryQueryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByCategoryQueryHandler.Handle called with ${Query}", query);

            var products = await documentSession.Query<Product>()
                .Where(p => p.Category.Any(c => c.EqualsIgnoreCase(query.Category)))
                .ToListAsync();

            return new GetProductByCategoryQueryResult(products);
        }
    }
}

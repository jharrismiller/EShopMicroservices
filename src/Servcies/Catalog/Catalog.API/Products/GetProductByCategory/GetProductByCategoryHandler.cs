using JasperFx.Core;

namespace Catalog.API.Products.GetProductByCategory
{

    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryQueryResult>;


    public record GetProductByCategoryQueryResult(IEnumerable<Product> Products);

    internal class GetProductByCategoryQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryQueryResult>
    {
        public async Task<GetProductByCategoryQueryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await documentSession.Query<Product>()
                .Where(p => p.Category.Any(c => c.EqualsIgnoreCase(query.Category)))
                .ToListAsync();

            return new GetProductByCategoryQueryResult(products);
        }
    }
}

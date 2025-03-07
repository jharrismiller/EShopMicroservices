namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketQueryResult>;

public record GetBasketQueryResult(ShoppingCart Cart);

public class GetBasketHandler() : IQueryHandler<GetBasketQuery, GetBasketQueryResult>
{
    public async Task<GetBasketQueryResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {



        return new GetBasketQueryResult(new ShoppingCart("temp"));
    }
}

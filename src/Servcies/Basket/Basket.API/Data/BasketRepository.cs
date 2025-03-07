namespace Basket.API.Data;

public class BasketRepository(IDocumentSession documentSession) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var shoppingCart = await documentSession.LoadAsync<ShoppingCart>(userName, cancellationToken);

        if (shoppingCart is null)
            throw new BasketNotFoundException(userName);

        return shoppingCart;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        documentSession.Store(shoppingCart);
        await documentSession.SaveChangesAsync(cancellationToken);
        return shoppingCart;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        documentSession.Delete<ShoppingCart>(userName);
        await documentSession.SaveChangesAsync(cancellationToken);
        return true;
    }
}

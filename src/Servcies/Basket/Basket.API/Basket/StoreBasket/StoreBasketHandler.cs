namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketCommandResult>;

public record StoreBasketCommandResult(string UserName);

internal class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketCommandResult>
{
    public async Task<StoreBasketCommandResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        return new StoreBasketCommandResult(command.Cart.UserName);
    }
}


public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }

}

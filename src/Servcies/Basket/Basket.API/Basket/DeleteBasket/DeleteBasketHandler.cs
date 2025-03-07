namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketCommandResult>;
public record DeleteBasketCommandResult(bool IsSuccess);
public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketCommandResult>
{
    public async Task<DeleteBasketCommandResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {


        return new DeleteBasketCommandResult(true);
    }
}

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
    }
}

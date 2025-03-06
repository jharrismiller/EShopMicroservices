
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductCommandResult>;

public record DeleteProductCommandResult(bool IsSuccess);

internal class DeleteProductCommandHandler(IDocumentSession documentSession, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductCommandResult>
{
    public async Task<DeleteProductCommandResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {

        logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", command);

        documentSession.Delete<Product>(command.Id);

        await documentSession.SaveChangesAsync(cancellationToken);

        return new DeleteProductCommandResult(true);
    }
}


public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
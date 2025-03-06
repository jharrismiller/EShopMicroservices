
namespace Catalog.API.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductCommandResult>;

    public record DeleteProductCommandResult(bool IsSuccess);

    public class DeleteProductCommandHandler(IDocumentSession documentSession, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductCommandResult>
    {
        public async Task<DeleteProductCommandResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {

            logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", command);

            documentSession.Delete<Product>(command.Id);

            await documentSession.SaveChangesAsync(cancellationToken);

            return new DeleteProductCommandResult(true);
        }
    }
}

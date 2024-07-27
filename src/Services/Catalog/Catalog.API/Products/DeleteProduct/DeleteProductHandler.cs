using Catalog.API.Products.UpdateProduct;
using FluentValidation;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(
    Guid Id
    ) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Product ID is required");
    }
}

internal class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken ct)
    {

        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(ct);

        return new DeleteProductResult(true);
    }
}

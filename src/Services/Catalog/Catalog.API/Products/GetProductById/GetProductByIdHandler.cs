using Catalog.API.Exceptions;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await session.LoadAsync<Product>(query.Id, ct);

        _ = product ?? throw new ProductNotFoundException(query.Id);

        return new GetProductByIdResult(product);
    }
}

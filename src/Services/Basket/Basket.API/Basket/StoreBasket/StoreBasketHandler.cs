using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class StoreBasketHandler(
    IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountProto
    ) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken ct)
    {
        await DeductDiscount(discountProto, command, ct);

        await repository.StoreBasket(command.Cart, ct);

        return new StoreBasketResult(command.Cart.UserName);
    }

    private static async Task DeductDiscount(
        DiscountProtoService.DiscountProtoServiceClient discountProto, 
        StoreBasketCommand command, CancellationToken ct)
    {
        foreach (var item in command.Cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(
                new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: ct);
            item.Price -= coupon.Amount;
        }
    }
}

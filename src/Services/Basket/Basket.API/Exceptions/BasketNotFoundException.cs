namespace Basket.API.Exceptions;

internal class BasketNotFoundException : NotFoundException
{
    public BasketNotFoundException(string userName) : base("Basket", userName)
    {
    }

    public BasketNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
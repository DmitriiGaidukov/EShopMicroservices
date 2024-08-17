namespace Basket.API.Models;

public class ShoppingCart
{
    public required string UserName { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public ShoppingCart()
    {
        
    }
}

namespace Shop.Application.Cart.Models;
public class CartProductDetailsDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Quantity { get; set; }
}

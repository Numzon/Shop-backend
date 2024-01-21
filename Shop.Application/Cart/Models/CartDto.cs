namespace Shop.Application.Cart.Models;
public class CartDto
{
    public Guid CartId { get; set; }
    public DateTime Created { get; set; }

    public IList<CartProductDto> Products { get; set; } = new List<CartProductDto>();
}

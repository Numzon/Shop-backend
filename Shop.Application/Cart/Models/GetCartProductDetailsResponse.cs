namespace Shop.Application.Cart.Models;
public class GetCartProductDetailsResponse
{
    public IEnumerable<CartProductDetailsDto> CartProducts { get; set; } = new List<CartProductDetailsDto>();
}

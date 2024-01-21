namespace Shop.Application.Products.Models;
public class GetSearchBarResponse
{
    public IEnumerable<SearchBarItemDto> Data { get; set; } = new List<SearchBarItemDto>();
}

using Shop.Application.Common.Models;

namespace Shop.Application.Category.Models;
public class GetCategoriesResponseDto
{
    public IEnumerable<CategoryListItemDto> Data { get; set; } = new List<CategoryListItemDto>();
    public required MetaDto Meta { get; set; }
}



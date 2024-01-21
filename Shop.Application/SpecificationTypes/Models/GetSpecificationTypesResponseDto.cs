using Shop.Application.Common.Models;

namespace Shop.Application.SpecificationTypes.Models;
public class GetSpecificationTypesResponseDto 
{
    public IEnumerable<SpecificationTypeListItemDto> Data { get; set; } = new List<SpecificationTypeListItemDto>();
    public required MetaDto Meta { get; set; } 
}

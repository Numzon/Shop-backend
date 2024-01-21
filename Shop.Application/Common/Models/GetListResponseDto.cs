namespace Shop.Application.Common.Models;
public class GetListResponseDto<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public required MetaDto Meta { get; set; }
}

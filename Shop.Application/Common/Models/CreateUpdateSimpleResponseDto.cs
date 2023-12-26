namespace Shop.Application.Common.Models;
public class CreateUpdateSimpleResponseDto
{
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; } = null!;
}

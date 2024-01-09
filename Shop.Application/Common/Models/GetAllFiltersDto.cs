namespace Shop.Application.Common.Models;
public class GetAllFiltersDto
{
    public string? SearchString { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 20;
}

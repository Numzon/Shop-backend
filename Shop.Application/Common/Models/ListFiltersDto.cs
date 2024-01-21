namespace Shop.Application.Common.Models;
public class ListFiltersDto
{
    public string? SearchString { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 20;
}

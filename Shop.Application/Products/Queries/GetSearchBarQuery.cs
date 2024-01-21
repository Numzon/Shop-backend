using MediatR;
using Shop.Application.Products.Models;

namespace Shop.Application.Products.Queries;
public sealed class GetSearchBarQuery : IRequest<GetSearchBarResponse>
{
    public string? SearchString { get; set; }
    public int PageSize { get; set; } = 20;
}

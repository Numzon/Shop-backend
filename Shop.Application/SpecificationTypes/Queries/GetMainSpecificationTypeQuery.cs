using MediatR;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.Category.Queries;
public sealed class GetMainSpecificationTypeQuery : IRequest<List<SpecificationTypeListItemDto>>
{
    public string? SearchString { get; set; }
}

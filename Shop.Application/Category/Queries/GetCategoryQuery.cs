using MediatR;
using Shop.Application.Category.Models;

namespace Shop.Application.Category.Queries;
public sealed class GetCategoryQuery : IRequest<CategoryDto>
{
    public Guid Id { get; set; }
}

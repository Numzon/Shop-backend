using MediatR;
using Shop.Application.Category.Models;

namespace Shop.Application.Category.Commands.CreateCategory;
public sealed class CreateCategoryCommand : IRequest<CategoryDto>
{
    public required string Name { get; set; }

    public IReadOnlyCollection<CreateSubcategoryDto> Subcategories { get; set; } = new List<CreateSubcategoryDto>();
}

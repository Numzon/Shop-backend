using MediatR;
using Shop.Application.Category.Models;

namespace Shop.Application.Category.Commands.EditCategory;
public sealed class EditCategoryCommand : IRequest
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }

    public IReadOnlyCollection<EditSubcategoryDto> Subcategories { get; set; } = new List<EditSubcategoryDto>();
}

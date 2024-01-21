using MediatR;

namespace Shop.Application.Category.Commands.DeleteCategory;
public sealed class DeleteCategoryCommand : IRequest
{
    public Guid Id { get; set; }
}

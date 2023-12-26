using Mapster;
using MediatR;
using Shop.Application.Category.Commands.CreateCategory;
using Shop.Application.Category.Models;
using Shop.Application.Common.Interfaces;
using Shop.Domain.Entities;

namespace Shop.Application.Category.Handlers;
public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = request.Adapt<ProductCategory>();

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return category.Adapt<CategoryDto>();
    }
}

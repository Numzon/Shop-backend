using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Commands.DeleteCategory;
using Shop.Application.Common.Interfaces;
using Shop.Domain.Entities;

namespace Shop.Application.Category.Handlers;
public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _context.Categories.Where(x => x.Id == request.Id).Include(x => x.Subcategories).FirstOrDefault();

        if (category is null)
        {
            throw new InvalidOperationException($"Cannot find category with given Id: {request.Id}");
        }

        _context.Categories.Entry(category).State = EntityState.Deleted;
        await SetSubcategoriesAsDeleted(category, cancellationToken);

        await _context.SaveChangesAsync();
    }

    public async Task SetSubcategoriesAsDeleted(ProductCategory category, CancellationToken cancellationToken)
    {
        var subcategories = await _context.Categories.Where(x => x.ParentCategoryId == category.Id).ToListAsync(cancellationToken);
        if (subcategories != null && subcategories.Any())
        {
            foreach (var item in subcategories)
            {
                _context.Categories.Entry(item).State = EntityState.Deleted;
                await SetSubcategoriesAsDeleted(item, cancellationToken);
            }
        }
    }
}

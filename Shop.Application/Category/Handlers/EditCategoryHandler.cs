using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Commands.EditCategory;
using Shop.Application.Category.Models;
using Shop.Application.Common.Interfaces;
using Shop.Domain.Entities;

namespace Shop.Application.Category.Handlers;
public sealed class EditCategoryHandler : IRequestHandler<EditCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public EditCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Include(x => x.Subcategories)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            throw new InvalidOperationException($"Category cannot be found. Category Id: {request.Id}");
        }

        var added = request.Subcategories.Where(x => x.Id == Guid.Empty).ToList(); 
        var deleted = category.Subcategories.Where(x => !request.Subcategories.Any(z => z.Id == x.Id)).ToList();
        var modified = request.Subcategories.Where(x => category.Subcategories.Any(z => z.Id == x.Id)).ToList();

        category = request.Adapt(category);
        _context.Categories.Entry(category).State = EntityState.Modified;

        MapAndSetStateAddedForEntities(added, category.Id);
        SetDeletedStateForRemovedEntities(deleted);
        AdaptModifiedEntitiesAndSetTheirStateAsModified(modified, category.Subcategories);

        await _context.SaveChangesAsync();
    }

    public void MapAndSetStateAddedForEntities(IEnumerable<EditSubcategoryDto> added, Guid parentCategoryId)
    {
        foreach (var item in added)
        {
            var subcategory = item.Adapt<ProductCategory>();
            subcategory.ParentCategoryId = parentCategoryId;
            _context.Categories.Entry(subcategory).State = EntityState.Added;
        }
    }

    public void SetDeletedStateForRemovedEntities(IEnumerable<ProductCategory> deleted)
    {
        foreach (var item in deleted)
        {
            _context.Categories.Entry(item).State = EntityState.Deleted;
            if (item.Subcategories.Any())
            {
                SetDeletedStateForRemovedEntities(item.Subcategories);
            }
        }
    }

    public void AdaptModifiedEntitiesAndSetTheirStateAsModified(IEnumerable<EditSubcategoryDto> modified, IEnumerable<ProductCategory> existingSubcategories)
    {
        foreach (var item in modified)
        {
            var subcategory = existingSubcategories.FirstOrDefault(x => x.Id == item.Id);
            subcategory = item.Adapt(subcategory);
            if (subcategory is not null)
            {
                _context.Categories.Entry(subcategory).State = EntityState.Modified;
            }
        }
    }
}

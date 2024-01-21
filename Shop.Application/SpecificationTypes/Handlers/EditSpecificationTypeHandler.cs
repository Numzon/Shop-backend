using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Commands.EditCategory;
using Shop.Application.Category.Models;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationTypes.Commands.EditSpecificationType;
using Shop.Application.SpecificationTypes.Models;
using Shop.Domain.Entities;

namespace Shop.Application.SpecificationTypes.Handlers;
public sealed class EditSpecificationTypeHandler : IRequestHandler<EditSpecificationTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public EditSpecificationTypeHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditSpecificationTypeCommand request, CancellationToken cancellationToken)
    {
        var specificationType = await _context.SpecificationTypes
            .Include(x => x.Subtypes)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (specificationType is null)
        {
            throw new InvalidOperationException($"Category cannot be found. Category Id: {request.Id}");
        }

        var added = request.Subtypes.Where(x => x.Id == Guid.Empty).ToList(); 
        var deleted = specificationType.Subtypes.Where(x => !request.Subtypes.Any(z => z.Id == x.Id)).ToList();
        var modified = request.Subtypes.Where(x => specificationType.Subtypes.Any(z => z.Id == x.Id)).ToList();

        specificationType = request.Adapt(specificationType);
        _context.SpecificationTypes.Entry(specificationType).State = EntityState.Modified;

        MapAndSetStateAddedForEntities(added, specificationType.Id);
        SetDeletedStateForRemovedEntities(deleted);
        AdaptModifiedEntitiesAndSetTheirStateAsModified(modified, specificationType.Subtypes);

        await _context.SaveChangesAsync();
    }

    public void MapAndSetStateAddedForEntities(IEnumerable<EditSubspecificationTypeDto> added, Guid parentCategoryId)
    {
        foreach (var item in added)
        {
            var specificationTypes = item.Adapt<SpecificationType>();
            specificationTypes.ParentId = parentCategoryId;
            _context.SpecificationTypes.Entry(specificationTypes).State = EntityState.Added;
        }
    }

    public void SetDeletedStateForRemovedEntities(IEnumerable<SpecificationType> deleted)
    {
        foreach (var item in deleted)
        {
            _context.SpecificationTypes.Entry(item).State = EntityState.Deleted;
            if (item.Subtypes.Any())
            {
                SetDeletedStateForRemovedEntities(item.Subtypes);
            }
        }
    }

    public void AdaptModifiedEntitiesAndSetTheirStateAsModified(IEnumerable<EditSubspecificationTypeDto> modified, IEnumerable<SpecificationType> existingSubtypes)
    {
        foreach (var item in modified)
        {
            var subtype = existingSubtypes.FirstOrDefault(x => x.Id == item.Id);
            subtype = item.Adapt(subtype);
            if (subtype is not null)
            {
                _context.SpecificationTypes.Entry(subtype).State = EntityState.Modified;
            }
        }
    }
}

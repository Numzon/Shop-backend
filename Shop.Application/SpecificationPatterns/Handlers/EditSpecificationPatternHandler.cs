using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Models;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationPatterns.Commands.EditSpecificationPattern;
using Shop.Application.SpecificationPatterns.Models;
using Shop.Domain.Entities;

namespace Shop.Application.SpecificationPatterns.Handlers;
public sealed class EditSpecificationPatternHandler : IRequestHandler<EditSpecificationPatternCommand>
{
    private readonly IApplicationDbContext _context;

    public EditSpecificationPatternHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditSpecificationPatternCommand request, CancellationToken cancellationToken)
    {
        var pattern = await _context.SpecificationPatterns
            .Include(x => x.SpecificationPatternSpecificationTypes)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (pattern is null)
        {
            throw new InvalidOperationException($"Specification pattern with given id cannont be found! Pattern Id: {request.Id}");
        }

        var added = request.Types.Where(x => x.Id == Guid.Empty).ToList();
        var deleted = pattern.SpecificationPatternSpecificationTypes.Where(x => !request.Types.Any(z => z.Id == x.Id)).ToList();
        var modified = request.Types.Where(x => pattern.SpecificationPatternSpecificationTypes.Any(z => z.Id == x.Id)).ToList();

        pattern = request.Adapt(pattern);
        _context.SpecificationPatterns.Entry(pattern).State = EntityState.Modified;

        MapAndSetStateAddedForEntities(added, pattern.Id);
        SetDeletedStateForRemovedEntities(deleted);
        AdaptModifiedEntitiesAndSetTheirStateAsModified(modified, pattern.SpecificationPatternSpecificationTypes);

        await _context.SaveChangesAsync();
    }
    public void MapAndSetStateAddedForEntities(IEnumerable<SimpleSpecificationPatternSpecificationTypeDto> added, Guid patternId)
    {
        foreach (var item in added)
        {
            var patternType = item.Adapt<SpecificationPatternSpecificationType>();
            patternType.SpecificationPatternId = patternId;
            _context.SpecificationPatternSpecificationTypes.Entry(patternType).State = EntityState.Added;
        }
    }

    public void SetDeletedStateForRemovedEntities(IEnumerable<SpecificationPatternSpecificationType> deleted)
    {
        foreach (var item in deleted)
        {
            _context.SpecificationPatternSpecificationTypes.Entry(item).State = EntityState.Deleted;
        }
    }

    public void AdaptModifiedEntitiesAndSetTheirStateAsModified(IEnumerable<SimpleSpecificationPatternSpecificationTypeDto> modified, IEnumerable<SpecificationPatternSpecificationType> existingEntities)
    {
        foreach (var item in modified)
        {
            var entity = existingEntities.FirstOrDefault(x => x.Id == item.Id);
            entity = item.Adapt(entity);
            if (entity is not null)
            {
                _context.SpecificationPatternSpecificationTypes.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}

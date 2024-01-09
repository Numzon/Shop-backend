using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationTypes.Commands.DeleteSpecificationType;
using Shop.Domain.Entities;

namespace Shop.Application.SpecificationTypes.Handlers;
public sealed class DeleteSpecificationTypeHandler : IRequestHandler<DeleteSpecificationTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSpecificationTypeHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSpecificationTypeCommand request, CancellationToken cancellationToken)
    {
        var specificationType = _context.SpecificationTypes.Where(x => x.Id == request.Id).FirstOrDefault();

        if (specificationType is null)
        {
            throw new InvalidOperationException($"Cannot find category with given Id: {request.Id}");
        }

        _context.SpecificationTypes.Entry(specificationType).State = EntityState.Deleted;
        await SetSpecificationTypesAsDeleted(specificationType, cancellationToken);

        await _context.SaveChangesAsync();
    }

    public async Task SetSpecificationTypesAsDeleted(SpecificationType specificationType, CancellationToken cancellationToken)
    {
        var subtypes = await _context.SpecificationTypes.Where(x => x.ParentId == specificationType.Id).ToListAsync(cancellationToken);
        if (subtypes != null && subtypes.Any())
        {
            foreach (var item in subtypes)
            {
                _context.SpecificationTypes.Entry(item).State = EntityState.Deleted;
                await SetSpecificationTypesAsDeleted(item, cancellationToken);
            }
        }
    }
}

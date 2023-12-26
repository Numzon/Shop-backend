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
        var specificationType = _context.SpecificationTypes.Where(x => x.Id == request.Id).Include(x => x.Subtypes).FirstOrDefault();

        if (specificationType is null)
        {
            throw new InvalidOperationException($"Cannot find category with given Id: {request.Id}");
        }

        _context.SpecificationTypes.Entry(specificationType).State = EntityState.Deleted;
        SetSpecificationTypesAsDeleted(specificationType.Subtypes);

        await _context.SaveChangesAsync();
    }

    public void SetSpecificationTypesAsDeleted(IEnumerable<SpecificationType> subtypes)
    {
        if (subtypes != null && subtypes.Any())
        {
            foreach (var item in subtypes)
            {
                _context.SpecificationTypes.Entry(item).State = EntityState.Deleted;
                SetSpecificationTypesAsDeleted(item.Subtypes);
            }
        }
    }
}

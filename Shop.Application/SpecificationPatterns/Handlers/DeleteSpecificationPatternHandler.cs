using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationPatterns.Commands.DeleteSpecificationPattern;

namespace Shop.Application.SpecificationPatterns.Handlers;
public sealed class DeleteSpecificationPatternHandler : IRequestHandler<DeleteSpecificationPatternCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSpecificationPatternHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSpecificationPatternCommand request, CancellationToken cancellationToken)
    {
        var pattern = await _context.SpecificationPatterns
            .Include(x => x.SpecificationPatternSpecificationTypes)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (pattern is null)
        {
            throw new InvalidOperationException($"Specification patternt with given Id cannot be found! Id: {request.Id}");
        }

        foreach (var item in pattern.SpecificationPatternSpecificationTypes)
        {
            _context.SpecificationPatternSpecificationTypes.Entry(item).State = EntityState.Deleted;
        }

        _context.SpecificationPatterns.Entry(pattern).State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }
}

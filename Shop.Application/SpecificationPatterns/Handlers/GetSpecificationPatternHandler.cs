using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationPatterns.Models;
using Shop.Application.SpecificationPatterns.Queries;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.SpecificationPatterns.Handlers;
public sealed class GetSpecificationPatternHandler : IRequestHandler<GetSpecificationPatternQuery, SpecificationPatternDto>
{
    private readonly IApplicationDbContext _context;

    public GetSpecificationPatternHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecificationPatternDto> Handle(GetSpecificationPatternQuery request, CancellationToken cancellationToken)
    {
        var pattern = await _context.SpecificationPatterns
            .Include(x => x.SpecificationPatternSpecificationTypes)
            .ThenInclude(z => z.SpecificationType)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (pattern is null)
        {
            throw new InvalidOperationException($"Specification pattern with given Id cannot be found! Id: {request.Id}");
        }

        return pattern.Adapt<SpecificationPatternDto>();
    }
}

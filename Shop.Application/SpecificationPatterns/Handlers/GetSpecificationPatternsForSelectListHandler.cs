using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationPatterns.Models;
using Shop.Application.SpecificationPatterns.Queries;

namespace Shop.Application.SpecificationPatterns.Handlers;
public sealed class GetSpecificationPatternsForSelectListHandler : IRequestHandler<GetSpecificationPatternsForSelectListQuery, IEnumerable<SimpleSpecificationPatternDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSpecificationPatternsForSelectListHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SimpleSpecificationPatternDto>> Handle(GetSpecificationPatternsForSelectListQuery request, CancellationToken cancellationToken)
    {
        var patterns = await _context.SpecificationPatterns
            .Select(x => new SimpleSpecificationPatternDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync(cancellationToken);

        return patterns;    
    }
}

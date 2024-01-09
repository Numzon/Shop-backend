using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.Common.Models;
using Shop.Application.SpecificationPatterns.Models;
using Shop.Application.SpecificationPatterns.Queries;

namespace Shop.Application.SpecificationPatterns.Handlers;
public sealed class GetAllSpecificationPatternHandler : IRequestHandler<GetAllSpecificationPatternsQuery, GetListResponseDto<SpecificationPatternListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSpecificationPatternHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetListResponseDto<SpecificationPatternListItemDto>> Handle(GetAllSpecificationPatternsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SpecificationPatterns
            .Where(x => string.IsNullOrEmpty(request.SearchString) || x.Name.Contains(request.SearchString));

        var patterns = await query
            .Select(x => new SpecificationPatternListItemDto
            {
                Id = x.Id,
                Name = x.Name,
            })
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var meta = new MetaDto
        {
            Total = await query.CountAsync(cancellationToken)
        };

        return new GetListResponseDto<SpecificationPatternListItemDto>
        {
            Data = patterns,
            Meta = meta
        };
    }
}

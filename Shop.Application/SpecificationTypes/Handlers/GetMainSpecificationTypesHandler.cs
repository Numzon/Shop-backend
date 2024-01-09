using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Queries;
using Shop.Application.Common.Interfaces;
using Shop.Application.Common.Models;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.SpecificationTypes.Handlers;
public sealed class GetMainSpecificationTypesHandler : IRequestHandler<GetAllMainSpecificationTypesQuery, GetListResponseDto<SpecificationTypeListItemDto>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetMainSpecificationTypesHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<GetListResponseDto<SpecificationTypeListItemDto>> Handle(GetAllMainSpecificationTypesQuery request, CancellationToken cancellationToken)
    {
        var query = _applicationDbContext.SpecificationTypes
            .Where(x => x.ParentId == null
                && (string.IsNullOrEmpty(request.SearchString) || x.Name.Contains(request.SearchString)));

        var types = await query
            .Include(x => x.Subtypes)
            .Select(x => new SpecificationTypeListItemDto()
            {
                Id  = x.Id,
                Name = x.Name,
                SubspecificationTypeCount = x.Subtypes.Count
            })
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);


        var meta = new MetaDto
        {
            Total = await query.CountAsync(cancellationToken)
        };

        return new GetListResponseDto<SpecificationTypeListItemDto>
        {
            Data = types,
            Meta = meta
        };
    }
}

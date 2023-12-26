using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Queries;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.SpecificationTypes.Handlers;
public sealed class GetMainSpecificationTypesHandler : IRequestHandler<GetMainSpecificationTypeQuery, List<SpecificationTypeListItemDto>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetMainSpecificationTypesHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<SpecificationTypeListItemDto>> Handle(GetMainSpecificationTypeQuery request, CancellationToken cancellationToken)
    {
        var categories = await _applicationDbContext.SpecificationTypes
            .Include(x => x.Subtypes)
            .Where(x => x.ParentId == null
                && (string.IsNullOrEmpty(request.SearchString) || x.Name.Contains(request.SearchString)))
            .Select(x => new SpecificationTypeListItemDto()
            {
                Name = x.Name,
                SubspecificationTypeCount = x.Subtypes.Count
            }).ToListAsync(cancellationToken);

        return categories;
    }
}

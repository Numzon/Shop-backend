using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Models;
using Shop.Application.Category.Queries;
using Shop.Application.Common.Interfaces;
using Shop.Application.Common.Models;

namespace Shop.Application.Category.Handlers;
public sealed class GetMainCategoriesHandler : IRequestHandler<GetMainCategoriesQuery, GetListResponseDto<CategoryListItemDto>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetMainCategoriesHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<GetListResponseDto<CategoryListItemDto>> Handle(GetMainCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _applicationDbContext.Categories
            .Include(x => x.Subcategories)
            .Where(x => x.ParentCategoryId == null
                && (string.IsNullOrEmpty(request.SearchString) || x.Name.Contains(request.SearchString)))
            .Select(x => new CategoryListItemDto()
            {
                Id = x.Id,
                Name = x.Name,
                SubcategoriesCount = x.Subcategories.Count
            })
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var meta = new MetaDto
        {
            Total = await _applicationDbContext.Categories.Where(x => x.ParentCategoryId == null).CountAsync()
        };

        return new GetListResponseDto<CategoryListItemDto>
        {
            Data = categories,
            Meta = meta
        };
    }
}

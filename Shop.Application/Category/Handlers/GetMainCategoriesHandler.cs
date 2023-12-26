using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Models;
using Shop.Application.Category.Queries;
using Shop.Application.Common.Interfaces;

namespace Shop.Application.Category.Handlers;
public sealed class GetMainCategoriesHandler : IRequestHandler<GetMainCategoriesQuery, List<CategoryListItemDto>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetMainCategoriesHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<CategoryListItemDto>> Handle(GetMainCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _applicationDbContext.Categories
            .Include(x => x.Subcategories)
            .Where(x => x.ParentCategoryId == null
                && (string.IsNullOrEmpty(request.SearchString) || x.Name.Contains(request.SearchString)))
            .Select(x => new CategoryListItemDto()
            {
                Name = x.Name,
                SubcategoriesCount = x.Subcategories.Count
            }).ToListAsync(cancellationToken);

        return categories;
    }
}

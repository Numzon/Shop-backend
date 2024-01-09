using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Models;
using Shop.Application.Category.Queries;
using Shop.Application.Common.Interfaces;

namespace Shop.Application.Category.Handlers;
public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Include(x => x.Subcategories)
            .Include(x => x.ParentCategory)
            .Include(x => x.SpecificationPattern)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (category is null)
        {
            throw new InvalidOperationException($"Category with given Id cannot be found! Id: {request.Id}");
        }

        return category.Adapt<CategoryDto>();
    }
}

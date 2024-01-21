using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Models;
using Shop.Application.Common.Interfaces;
using Shop.Application.Products.Models;
using Shop.Application.Products.Queries;
using Shop.Domain.Entities;

namespace Shop.Application.Products.Handlers;
public sealed class GetProductDetailsHandler : IRequestHandler<GetProductDetailsQuery, ProductDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductDetailsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDetailsDto> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {   
        var product = await _context
            .Products
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product == null)
        {
            throw new InvalidOperationException($"Product cannot be found! Id: {request.Id}");
        }

        var categories = await GetCategoryWithHisParents(product.CategoryId);

        var productDetailsDto = product.Adapt<ProductDetailsDto>();

        productDetailsDto.CategoriesPath = categories.Reverse().Adapt<IEnumerable<SimpleCategoryDto>>();

        return productDetailsDto;
    }
    private async Task<IEnumerable<ProductCategory>> GetCategoryWithHisParents(Guid categoryId)
    {
        var category = await _context
            .Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
        
        if (category == null)
        {
            throw new InvalidOperationException($"Category cannot be found! Id: {categoryId}");
        }

        var categoriesList = new List<ProductCategory>
        {
            category
        };

        if (category.ParentCategoryId != null)
        {
            categoriesList.AddRange(await GetCategoryWithHisParents(category.ParentCategoryId.Value));
        }

        return categoriesList;
    }
}
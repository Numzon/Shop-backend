using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Category.Models;
using Shop.Application.Common.Interfaces;
using Shop.Application.Common.Models;
using Shop.Application.Products.Models;
using Shop.Application.Products.Queries;

namespace Shop.Application.Products.Handlers;
public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, GetListResponseDto<ProductListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetListResponseDto<ProductListItemDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context
            .Products
            .Where(x => string.IsNullOrEmpty(request.SearchString) || x.Name.Contains(request.SearchString));

        var products = await query
            .Include(x => x.Category)
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ProductListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Category = new SimpleCategoryDto
                {
                    Id = x.Category.Id,
                    Name = x.Name,
                }
            }).ToListAsync(cancellationToken);

        var meta = new MetaDto
        {
            Total = await query.CountAsync(cancellationToken)
        };

        return new GetListResponseDto<ProductListItemDto>
        {
            Data = products,
            Meta = meta
        };
    }
}

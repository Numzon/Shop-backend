using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.Products.Models;
using Shop.Application.Products.Queries;

namespace Shop.Application.Products.Handlers;
public sealed class GetProductHandler : IRequestHandler<GetProductQuery, ProductDto>
{
	private readonly IApplicationDbContext _context;

	public GetProductHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
	{
		var product = await _context
			.Products
			.Include(x => x.Category)
			.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (product is null)
        {
            throw new InvalidOperationException($"Product with given Id cannot be found! Id: {request.Id}");
        }

		return product.Adapt<ProductDto>();
    }
}

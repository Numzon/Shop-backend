using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationTypes.Models;
using Shop.Application.SpecificationTypes.Queries;

namespace Shop.Application.SpecificationTypes.Handlers;
public sealed class GetSpecificationTypeHandler : IRequestHandler<GetSpecificationTypeQuery, SpecificationTypeDto>
{
    private readonly IApplicationDbContext _context;

    public GetSpecificationTypeHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecificationTypeDto> Handle(GetSpecificationTypeQuery request, CancellationToken cancellationToken)
    {
        var type = await _context.SpecificationTypes
            .Include(x => x.Parent)
            .Include(x => x.Subtypes)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (type is null)
        {
            throw new InvalidOperationException($"Specification type with given Id cannot be found! Id: {request.Id}");
        }

        return type.Adapt<SpecificationTypeDto>();
    }
}

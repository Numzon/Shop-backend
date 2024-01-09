using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationPatterns.Models;
using Shop.Application.SpecificationTypes.Queries;
using Shop.Domain.Entities;

namespace Shop.Application.SpecificationTypes.Handlers;
public sealed class GetSpecificationTypesForSelectListHandler : IRequestHandler<GetSpecificationTypesForSelectListQuery, IEnumerable<SimpleSpecificationTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private const string SEPARATOR = " > ";

    public GetSpecificationTypesForSelectListHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SimpleSpecificationTypeDto>> Handle(GetSpecificationTypesForSelectListQuery request, CancellationToken cancellationToken)
    {
        var selectListItems = new List<SimpleSpecificationTypeDto>();

        var types = await _context.SpecificationTypes
            .ToListAsync(cancellationToken);

        var lowestSubtypes = types.Where(x => !x.Subtypes.Any()).ToList();

        foreach (var item in lowestSubtypes)
        {
            var name = GetFullnameName(item, types);
            selectListItems.Add(new SimpleSpecificationTypeDto
            {
                Id = item.Id,
                Name = name,
            });
        }

        return selectListItems;
    }

    private string GetFullnameName(SpecificationType type, List<SpecificationType> allTypes)
    {
        var name = type.Name;
        if (type.ParentId == null)
        {
            return name;
        }

        var parent = allTypes.FirstOrDefault(x => x.Id == type.ParentId);
        if (parent == null) 
        {
            return name; 
        }

        return string.Join(SEPARATOR, GetFullnameName(parent, allTypes), name);
    }
}

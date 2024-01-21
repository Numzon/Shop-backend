using Mapster;
using MediatR;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationPatterns.Commands.CreateSpecificationPattern;
using Shop.Application.SpecificationPatterns.Models;
using Shop.Domain.Entities;

namespace Shop.Application.SpecificationPatterns.Handlers;
public sealed class CreateSpecificationPatternHandler : IRequestHandler<CreateSpecificationPatternCommand, SimpleSpecificationPatternDto>
{
    private readonly IApplicationDbContext _context;

    public CreateSpecificationPatternHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SimpleSpecificationPatternDto> Handle(CreateSpecificationPatternCommand request, CancellationToken cancellationToken)
    {
        var patternTypeEntities = new List<SpecificationPatternSpecificationType>();
        foreach (var type in request.Types)
        {
            patternTypeEntities.Add(new SpecificationPatternSpecificationType
            {
                SpecificationTypeId = type.Id,
            });
        }

        var pattern = new SpecificationPattern
        {
            Name = request.Name,
            SpecificationPatternSpecificationTypes = patternTypeEntities
        };

        _context.SpecificationPatterns.Add(pattern);
        await _context.SaveChangesAsync(cancellationToken);

        return pattern.Adapt<SimpleSpecificationPatternDto>();
    }
}

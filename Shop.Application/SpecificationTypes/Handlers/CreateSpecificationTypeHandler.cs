using Mapster;
using MediatR;
using Shop.Application.Common.Interfaces;
using Shop.Application.SpecificationTypes.Commands.CreateSpecificationType;
using Shop.Application.SpecificationTypes.Models;
using Shop.Domain.Entities;

namespace Shop.Application.SpecificationTypes.Handlers;
public sealed class CreateSpecificationTypeHandler : IRequestHandler<CreateSpecificationTypeCommand, SimpleSpecificationTypeDto>
{
    private readonly IApplicationDbContext _context;

    public CreateSpecificationTypeHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SimpleSpecificationTypeDto> Handle(CreateSpecificationTypeCommand request, CancellationToken cancellationToken)
    {
        var specificationType = request.Adapt<SpecificationType>();

        _context.SpecificationTypes.Add(specificationType);
        await _context.SaveChangesAsync(cancellationToken);

        return specificationType.Adapt<SimpleSpecificationTypeDto>();
    }
}

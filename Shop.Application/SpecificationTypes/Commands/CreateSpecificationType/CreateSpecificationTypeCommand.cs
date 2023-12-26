using MediatR;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.SpecificationTypes.Commands.CreateSpecificationType;
public sealed class CreateSpecificationTypeCommand : IRequest<SpecificationTypeDto>
{
    public required string Name { get; set; }

    public IReadOnlyCollection<CreateSubspecificationTypeDto> Subtypes { get; set; } = new List<CreateSubspecificationTypeDto>();
}
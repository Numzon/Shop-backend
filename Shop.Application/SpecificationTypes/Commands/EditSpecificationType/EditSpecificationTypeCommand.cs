using MediatR;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.SpecificationTypes.Commands.EditSpecificationType;
public sealed class EditSpecificationTypeCommand : IRequest
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }

    public IReadOnlyCollection<EditSubspecificationTypeDto> Subtypes { get; set; } = new List<EditSubspecificationTypeDto>();
}

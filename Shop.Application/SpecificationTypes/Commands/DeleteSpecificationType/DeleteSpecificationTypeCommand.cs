using MediatR;

namespace Shop.Application.SpecificationTypes.Commands.DeleteSpecificationType;
public sealed class DeleteSpecificationTypeCommand : IRequest
{
    public Guid Id { get; set; }
}

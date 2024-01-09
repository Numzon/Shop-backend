using MediatR;

namespace Shop.Application.SpecificationPatterns.Commands.DeleteSpecificationPattern;
public sealed class DeleteSpecificationPatternCommand : IRequest
{
    public Guid Id { get; set; }
}

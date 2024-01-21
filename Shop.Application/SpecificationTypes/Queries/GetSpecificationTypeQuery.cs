using MediatR;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.SpecificationTypes.Queries;
public sealed class GetSpecificationTypeQuery : IRequest<SpecificationTypeDto>
{
    public Guid Id { get; set; }
}

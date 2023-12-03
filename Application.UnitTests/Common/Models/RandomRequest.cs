using MediatR;
using static Application.UnitTests.Common.Behaviours.UnhandledExceptionBehaviourTests;

namespace Application.UnitTests.Common.Models;
public sealed class RandomRequest : IRequest<RandomResponse>
{
    public bool ShouldThrowException { get; set; }
    public string? RandomStringProperty { get; set; }
}

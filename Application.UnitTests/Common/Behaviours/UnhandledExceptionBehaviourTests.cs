using Application.UnitTests.Common.Models;
using MediatR;
using Shop.Application.Common.Behaviours;

namespace Application.UnitTests.Common.Behaviours;
public sealed class UnhandledExceptionBehaviourTests
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public async Task Handle_RequestHandlerDelegateDoesntThrowException_ReturnsResultObject()
    {
        var request = _fixture.Build<RandomRequest>().With(x => x.ShouldThrowException, false).Create();
        var responseObj = _fixture.Create<RandomResponse>();
        var handler = new RandomRequestHandler(responseObj);
        var behaviour = new UnhandledExceptionBehaviour<RandomRequest, RandomResponse>();

        var response = await behaviour.Handle(request, async () => await handler.Handle(request, default), default);

        response.Should().NotBeNull();
        response.Success.Should().Be(responseObj.Success);
    }

    [Fact]
    public async Task Handle_RequestHandlerDelegateThrowsException_Exception()
    {
        var request = _fixture.Build<RandomRequest>().With(x => x.ShouldThrowException, true).Create();
        var responseObj = _fixture.Create<RandomResponse>();
        var handler = new RandomRequestHandler(responseObj);
        var behaviour = new UnhandledExceptionBehaviour<RandomRequest, RandomResponse>();

        var func = async () => await behaviour.Handle(request, async () => await handler.Handle(request, default), default);

        await func.Should().ThrowAsync<Exception>();
    }
}

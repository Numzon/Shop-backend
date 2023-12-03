using Application.UnitTests.Common.Models;
using FluentValidation;
using Shop.Application.Common.Behaviours;
using ValidationException = Shop.Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.Common.Behaviours;

public sealed class ValidationBehaviourTests
{
    private readonly Fixture _fixture;

    public ValidationBehaviourTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_ValidatorsListIsEmpty_ReturnsResponseObject()
    {
        var behavior = new ValidationBehaviour<RandomRequest,RandomResponse>(Enumerable.Empty<IValidator<RandomRequest>>());
        var request = _fixture.Build<RandomRequest>().With(x => x.ShouldThrowException, false).Create();
        var response = _fixture.Create<RandomResponse>();   
        var handler = new RandomRequestHandler(response);

        var result = await behavior.Handle(request, async () => await handler.Handle(request, default), default);

        result.Should().NotBeNull();
        result.Success.Should().Be(response.Success);
    }

    [Fact]
    public async Task Handle_ValidatorsExistButNonOfThemFails_ReturnsResponseObject()
    {
        var validator = new RandomRequestValidation();
        var behavior = new ValidationBehaviour<RandomRequest, RandomResponse>(new List<IValidator<RandomRequest>>() { validator });
        var request = _fixture.Build<RandomRequest>().With(x => x.ShouldThrowException, false).Create();
        var response = _fixture.Create<RandomResponse>();
        var handler = new RandomRequestHandler(response);

        var result = await behavior.Handle(request, async () => await handler.Handle(request, default), default);

        result.Should().NotBeNull();
        result.Success.Should().Be(response.Success);
    }

    [Fact]
    public async Task Handle_ValidatorsExistAndOneOfThemFails_ThrowsValidationException()
    {
        var validator = new RandomRequestValidation();
        var behaviour = new ValidationBehaviour<RandomRequest, RandomResponse>(new List<IValidator<RandomRequest>>() { validator });
        var request = _fixture.Build<RandomRequest>().With(x => x.ShouldThrowException, false).Without(x => x.RandomStringProperty).Create();
        var response = _fixture.Create<RandomResponse>();
        var handler = new RandomRequestHandler(response);

        var func = async () => await behaviour.Handle(request, async () => await handler.Handle(request, default), default);

        await func.Should().ThrowAsync<ValidationException>();
    }
}

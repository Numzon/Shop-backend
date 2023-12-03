using MediatR;

namespace Application.UnitTests.Common.Models;
public sealed class RandomRequestHandler : IRequestHandler<RandomRequest, RandomResponse>
{
    private readonly RandomResponse _randomResponse;
    public RandomRequestHandler(RandomResponse randomResponse)
    {
        _randomResponse = randomResponse;   
    }

    public Task<RandomResponse> Handle(RandomRequest request, CancellationToken cancellationToken)
    {
        if (request.ShouldThrowException)
        {
            throw new Exception();
        }

        return Task.FromResult(_randomResponse);
    }
}
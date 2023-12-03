namespace Application.UnitTests.Common.Models;
public sealed class RandomResponse
{
    public RandomResponse()
    {

    }

    public RandomResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; set; }
}
using Shop.Application.Authentication.Models;
using Shop.Domain.Constants;

namespace Shop.Application.Common.Factories;
public static class AuthResultDtoFactory
{
    public static AuthResultDto InvalidTokensError()
    {
        return new AuthResultDto()
        {
            Success = false,
            Errors = new List<string>()
            {
                AuthenticationErrors.InvalidTokens
            }
        };
    }

    public static AuthResultDto ExpiredTokenError() {
        return new AuthResultDto()
        {
            Success = false,
            Errors = new List<string>()
            {
                AuthenticationErrors.ExpiredToken
            }
        };
    }

    public static AuthResultDto ServerError() {
        return new AuthResultDto()
        {
            Success = false,
            Errors = new List<string>()
            {
                AuthenticationErrors.ServerError
            }
        };
    }

    public static AuthResultDto InvalidPayloadError()
    {
        return new AuthResultDto()
        {
            Success = false,
            Errors = new List<string>()
            {
                AuthenticationErrors.InvalidPayload
            }
        };
    }
        
    public static AuthResultDto EmailInUseError()
    {
        return new AuthResultDto()
        {
            Success = false,
            Errors = new List<string>()
            {
                AuthenticationErrors.EmailIsAlreadyInUse
            }
        };
    }

    public static AuthResultDto IncorrectEmailOrPasswordError()
    {
        return new AuthResultDto()
        {
            Success = false,
            Errors = new List<string>()
            {
                AuthenticationErrors.IncorrectEmailOrPassword
            }
        };
    }
}

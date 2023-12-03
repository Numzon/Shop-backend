namespace Shop.Domain.Constants;
public struct AuthenticationErrors
{
    public const string InvalidTokens = "Invalid tokens";
    public const string ExpiredToken = "Expired token";
    public const string ServerError = "Server Error";
    public const string InvalidPayload = "Invalid payload";
    public const string EmailIsAlreadyInUse = "Email is already in use";
    public const string IncorrectEmailOrPassword = "Incorrect email or password";
}

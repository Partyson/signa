using ErrorOr;

namespace signa.Extensions;

public static class ErrorTypeExtensions
{
    internal static int ToStatusCode(this ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Conflict => 409,
            ErrorType.Forbidden => 403,
            ErrorType.Unauthorized => 401,
            ErrorType.Unexpected => 500,
            ErrorType.Validation => 400,
            ErrorType.NotFound => 404,
            ErrorType.Failure => 500,
            _ => 500
        };
    }
}
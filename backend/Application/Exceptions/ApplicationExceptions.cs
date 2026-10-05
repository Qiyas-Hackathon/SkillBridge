namespace SkillBridge.Application.Exceptions;


public sealed class RequestValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public RequestValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = new Dictionary<string, string[]>(errors);
    }
}


public sealed class ConflictException(string message) : Exception(message);


public sealed class AuthenticationFailedException(string message) : Exception(message);


public sealed class ForbiddenException(string message) : Exception(message);


public sealed class NotFoundException(string message) : Exception(message);

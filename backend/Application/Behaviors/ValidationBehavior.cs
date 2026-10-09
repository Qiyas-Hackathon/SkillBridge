using FluentValidation;
using MediatR;
using SkillBridge.Application.Exceptions;

namespace SkillBridge.Application.Behaviors;

/// <summary>
/// Runs every FluentValidation validator registered for the request before the handler executes.
/// Without this pipeline behavior the validators in /Validators are never invoked.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validatorList = validators.ToList();

        if (validatorList.Count == 0)
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(
            validatorList.Select(v => v.ValidateAsync(context, cancellationToken)));

        var errors = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).Distinct().ToArray());

        if (errors.Count > 0)
            throw new RequestValidationException(errors);

        return await next(cancellationToken);
    }
}

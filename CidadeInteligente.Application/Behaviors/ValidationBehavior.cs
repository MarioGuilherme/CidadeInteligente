using CidadeInteligente.Domain.Notifications;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CidadeInteligente.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, INotificationContext notification) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    private readonly INotificationContext _notification = notification;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(cancellationToken);

        ValidationContext<TRequest> context = new(request);

        ValidationResult[] results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        ValidationFailure[] failures = [.. results.SelectMany(r => r.Errors)];

        if (failures.Length > 0)
        {
            foreach (ValidationFailure error in failures)
                _notification.AddValidation(char.ToLowerInvariant(error.PropertyName[0]) + error.PropertyName[1..], error.ErrorMessage);

            return default!;
        }

        return await next(cancellationToken);
    }
}

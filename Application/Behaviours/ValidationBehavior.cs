using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Behaviours;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            ValidationResult[] validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            List<ValidationFailure> failures = validationResults.SelectMany(v => v.Errors).Where(f => f.ErrorCode != null).ToList();

            if (failures.Any())
                throw new Application.Exceptions.ValidationException(
                    failures.GroupBy(f => f.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(err => err.ErrorMessage).ToList())
                    );
        }

        return await next();
    }
}
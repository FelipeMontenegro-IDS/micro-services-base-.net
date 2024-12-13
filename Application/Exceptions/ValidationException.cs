using FluentValidation.Results;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    public Dictionary<string,string[]> Errors { get; set; }
    
    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();   
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)  : this
    {
        foreach (var failure in failures)
        {
            Errors.Add(failure.PropertyName, failure.ErrorMessage.Split(','));
        }
    }
}
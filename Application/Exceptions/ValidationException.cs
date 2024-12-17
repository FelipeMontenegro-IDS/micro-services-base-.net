using System.Collections;
using FluentValidation.Results;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    public Dictionary<string, List<string>> Errors { get; set; }

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new();
    }

    public ValidationException(Dictionary<string, List<string>> errors) : this()
    {
        Errors = errors;
    }
}
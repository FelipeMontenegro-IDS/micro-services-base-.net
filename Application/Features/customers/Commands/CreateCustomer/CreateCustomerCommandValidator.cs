using System.Data;
using FluentValidation;

namespace Application.Features.customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("is required.")
            .MaximumLength(80)
            .WithMessage("name must not exceed {MaxLength} characters.");

        RuleFor(p => p.Email)
            .NotEmpty()
            .WithMessage("is required.");

        RuleFor(p => p.Telefono)
            .NotEmpty()
            .WithMessage("is required.")
            .MaximumLength(9)
            .WithMessage("telefono must not exceed {MaxLength} characters.");

        RuleFor(p => p.Address)
            .NotEmpty()
            .WithMessage("is required.")
            .MaximumLength(80)
            .WithMessage("name must not exceed {MaxLength} characters.");

        RuleFor(p => p.Birthdate)
            .NotEmpty()
            .WithMessage("is required.");
    }
}
using FluentValidation;
using Rira.CrudTest.UseCases.Users.Create;
using Rira.CrudTest.UseCases.Users.Validators.Helpers;

namespace Rira.CrudTest.UseCases.Users.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
  public CreateUserCommandValidator()
  {
    RuleFor(command => command.NewModel.FirstName)
        .NotEmpty()
        .WithMessage("The User FirstName cannot be empty.");

    RuleFor(command => command.NewModel.LastName)
        .NotEmpty()
        .WithMessage("The User LastName cannot be empty.");

    RuleFor(command => command.NewModel.NationalCode)
        .NotEmpty()
        .WithMessage("The User NationalCode cannot be empty.");

    RuleFor(command => command.NewModel.NationalCode)
        .Must(IranianNationalCodeHelper.IsValid)
        .WithMessage("The User NationalCode is not valid.");

    RuleFor(command => command.NewModel.DateOfBirth)
        .NotEmpty()
        .WithMessage("The User DateOfBirth cannot be empty.");
  }
}

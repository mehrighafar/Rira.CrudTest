using FluentValidation;
using Rira.CrudTest.UseCases.Users.Create;
using Rira.CrudTest.UseCases.Users.Update;
using Rira.CrudTest.UseCases.Users.Validators.Helpers;

namespace Rira.CrudTest.UseCases.Users.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
  public UpdateUserCommandValidator()
  {
    RuleFor(command => command.NewModel.Id)
        .NotEmpty()
        .WithMessage("The User Id cannot be empty.")
        .Must(id => BeAValidGuid.IsValid(id.ToString()))
        .WithMessage("The User Id must be a valid GUID.");

    RuleFor(command => command.NewModel.FirstName)
        .NotEmpty()
        .WithMessage("The User FirstName cannot be empty.");

    RuleFor(command => command.NewModel.LastName)
        .NotEmpty()
        .WithMessage("The User LastName cannot be empty.");

    RuleFor(command => command.NewModel.NationalCode)
        .NotEmpty()
        .WithMessage("The User NationalCode cannot be empty.")
        .Must(IranianNationalCodeHelper.IsValid)
        .WithMessage("The User NationalCode is not valid.");

    RuleFor(command => command.NewModel.DateOfBirth)
        .NotEmpty()
        .WithMessage("The User DateOfBirth cannot be empty.");
  }
}

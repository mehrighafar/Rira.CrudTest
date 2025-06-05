using FluentValidation;
using Rira.CrudTest.UseCases.Users.Delete;
using Rira.CrudTest.UseCases.Users.Validators.Helpers;

namespace Rira.CrudTest.UseCases.Users.Validators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
  public DeleteUserCommandValidator()
  {
    RuleFor(query => query.Id.ToString())
        .NotEmpty()
        .WithMessage("The User Id cannot be empty.")
        .Must(id => BeAValidGuid.IsValid(id.ToString()))
        .WithMessage("The User Id must be a valid GUID.");
  }
}

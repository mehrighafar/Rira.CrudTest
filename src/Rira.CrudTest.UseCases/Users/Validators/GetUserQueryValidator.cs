using FluentValidation;
using Rira.CrudTest.UseCases.Users.Get;
using Rira.CrudTest.UseCases.Users.Validators.Helpers;

namespace Rira.CrudTest.UseCases.Users.Validators;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
  public GetUserQueryValidator()
  {
    RuleFor(query => query.Id.ToString())
        .NotEmpty()
        .WithMessage("The User Id cannot be empty.")
        .Must(id => BeAValidGuid.IsValid(id.ToString()))
        .WithMessage("The User Id must be a valid GUID.");
  }
}

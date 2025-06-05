namespace Rira.CrudTest.Core.Exceptions;

public sealed class UserNotUniqueNationalCodeException : BadRequestException
{
  public UserNotUniqueNationalCodeException(string nationalCode)
       : base($"The User with the national code {nationalCode} already exists.")
  {
  }
}

namespace Rira.CrudTest.Core.Exceptions;

public sealed class UserNotFoundException : NotFoundException
{
  public UserNotFoundException(Guid id)
      : base($"The User with the identifier {id} was not found.")
  {
  }
}

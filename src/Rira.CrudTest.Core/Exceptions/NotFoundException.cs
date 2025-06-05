using Grpc.Core;

namespace Rira.CrudTest.Core.Exceptions;

public abstract class NotFoundException : ApplicationException
{
  protected NotFoundException(string message)
      : base("Not Found", message, StatusCode.NotFound)
  {
  }
}

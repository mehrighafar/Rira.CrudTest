using Grpc.Core;

namespace Rira.CrudTest.Core.Exceptions;

public abstract class BadRequestException : ApplicationException
{
  protected BadRequestException(string message)
      : base("Bad Request", message, StatusCode.InvalidArgument)
  {
  }
}

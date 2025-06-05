using Grpc.Core;

namespace Rira.CrudTest.Core.Exceptions;

public abstract class ApplicationException : RpcException
{
  protected ApplicationException(string title, string message, StatusCode statusCode)
      : base(new Status(statusCode, message)) =>
      Title = title;

  public string Title { get; }
}

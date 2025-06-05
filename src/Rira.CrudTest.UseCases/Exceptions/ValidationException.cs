using ApplicationException = Rira.CrudTest.Core.Exceptions.ApplicationException;

namespace Rira.CrudTest.UseCases.Exceptions;

public sealed class ValidationException : ApplicationException
{
  public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
      : base("Validation Failure", "One or more validation errors occurred", Grpc.Core.StatusCode.InvalidArgument)
      => ErrorsDictionary = errorsDictionary;

  public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}

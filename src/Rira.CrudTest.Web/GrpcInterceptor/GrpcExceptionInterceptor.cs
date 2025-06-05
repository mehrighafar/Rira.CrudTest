using System.Text.Json;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Rira.CrudTest.Core.Exceptions;
using Rira.CrudTest.UseCases.Exceptions;
using ApplicationException = Rira.CrudTest.Core.Exceptions.ApplicationException;

namespace Rira.CrudTest.Web.GrpcInterceptor;
public class GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger) : Interceptor
{
  private readonly ILogger<GrpcExceptionInterceptor> _logger = logger;

  public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
      TRequest request,
      ServerCallContext context,
      UnaryServerMethod<TRequest, TResponse> continuation)
  {
    try
    {
      return await continuation(request, context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An exception occurred during gRPC call.");

      var statusCode = GetStatusCode(ex);
      var title = GetTitle(ex);
      var detail = ex.Message;
      var errors = GetErrors(ex);

      var errorResponse = new
      {
        title,
        status = (int)statusCode,
        detail,
        errors
      };

      var errorJson = JsonSerializer.Serialize(errorResponse);

      var trailers = new Metadata
            {
                { "error", errorJson }
            };

      throw new RpcException(new Status(statusCode, detail), trailers);
    }
  }

  private static StatusCode GetStatusCode(Exception exception) =>
      exception switch
      {
        UserNotUniqueNationalCodeException => StatusCode.InvalidArgument,
        UserNotFoundException => StatusCode.NotFound,
        NotFoundException => StatusCode.NotFound,
        BadRequestException => StatusCode.InvalidArgument,
        ValidationException => StatusCode.FailedPrecondition,
        _ => StatusCode.Internal
      };

  private static string GetTitle(Exception exception) =>
      exception switch
      {
        ApplicationException applicationException => applicationException.Title,
        _ => "Server Error"
      };

  private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
  {
    if (exception is ValidationException validationException)
    {
      return validationException.ErrorsDictionary;
    }

    return null;
  }
}

using System.Diagnostics;
using Serilog.Context;

namespace Rira.CrudTest.Web.Middleware;

internal sealed class OpenTelemetryContextMiddleware : IMiddleware
{
  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    using (LogContext.PushProperty("TraceId", Activity.Current?.TraceId.ToString()))
    {
      await next(context);
    }
  }
}

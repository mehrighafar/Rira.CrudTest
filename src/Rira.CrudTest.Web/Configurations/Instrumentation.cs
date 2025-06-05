using System.Diagnostics;

namespace Rira.CrudTest.Web.Configurations;

public class Instrumentation : IDisposable
{
  internal const string ActivitySourceName = "User-server";
  internal const string ActivitySourceVersion = "1.0.0";

  public Instrumentation()
  {
    ActivitySource = new ActivitySource(ActivitySourceName, ActivitySourceVersion);
  }

  public ActivitySource ActivitySource { get; }

  public void Dispose()
  {
    ActivitySource.Dispose();
    GC.SuppressFinalize(this);
  }
}

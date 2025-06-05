using Rira.CrudTest.BDD.Initialization;
using Rira.CrudTest.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace Rira.CrudTest.BDD;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
  private readonly ScenarioContext _context;
  public CustomWebApplicationFactory(ScenarioContext context)
  {
    _context = context;
  }
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.UseEnvironment("Test");

    builder.ConfigureServices(services =>
    {
      var ctx = services.SingleOrDefault(d => d.ServiceType == typeof(MongoDbExtension));
      services.Remove(ctx!);

      services.AddScoped(_ => _context.Get<MongoDbFixture>().testDatabase!);
    });

    var host = builder.Build();

    host.Start();
    return host;
  }
}

using Microsoft.Extensions.DependencyInjection;
using Rira.CrudTest.Infrastructure.Repositories;

namespace Rira.CrudTest.Infrastructure.Extensions;

public static class RepositoryExtension
{
  public static IServiceCollection AddRepository(this IServiceCollection services)
  {
    // Add User collection
    services.AddScoped<IUserRepository, MongoUserRepository>();
    return services;
  }
}


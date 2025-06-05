using Rira.CrudTest.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Serilog;
using Testcontainers.MongoDb;
using Xunit;

namespace Rira.CrudTest.FunctionalTests.Initialization;

public class MongoDbFixture : IAsyncLifetime
{
  private MongoDbContainer? _mongoDbContainer;
  public IUserRepository? userRepository;
  public IMongoDatabase? testDatabase;
  private static bool _serializersRegistered = false;

  public async Task InitializeAsync()
  {
    _mongoDbContainer = new MongoDbBuilder()
      .WithName("test-mongo")
      .WithImage("mongo:latest")
      .WithReuse(true)
      .Build();

    if (!_serializersRegistered)
    {
      BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
      _serializersRegistered = true;
    }

    await _mongoDbContainer.StartAsync();
    var client = new MongoClient(_mongoDbContainer!.GetConnectionString());
    testDatabase = client.GetDatabase("test");

    Log.Logger = new LoggerConfiguration()
           .WriteTo.TestCorrelator()
           .CreateLogger();

    userRepository = new MongoUserRepository(testDatabase);
  }

  public async Task DisposeAsync()
  {
    if (_mongoDbContainer != null)
    {
      await _mongoDbContainer.StopAsync();
      await _mongoDbContainer!.DisposeAsync();
    }
  }
}

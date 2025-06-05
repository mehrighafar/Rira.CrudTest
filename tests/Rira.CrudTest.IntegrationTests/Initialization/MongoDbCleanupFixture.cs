using Xunit;

namespace Rira.CrudTest.IntegrationTests.Initialization;

[Collection(nameof(TestCollection))]
public class MongoDbCleanupFixture(MongoDbFixture mongoDbFixture) : IAsyncLifetime
{
  private readonly MongoDbFixture _mongoDbFixture = mongoDbFixture;

  public async Task InitializeAsync()
  {
    var entities = await _mongoDbFixture.userRepository!.GetAllAsync();
    foreach (var entity in entities!)
      await _mongoDbFixture.userRepository!.RemoveAsync(entity.Id);
  }
  public async Task DisposeAsync()
  {
    await Task.CompletedTask;
  }
}

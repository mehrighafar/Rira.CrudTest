using Xunit;

namespace Rira.CrudTest.FunctionalTests.Initialization;

[Collection(nameof(MongoDbContainerCollection))]
public class MongoDbCleanupFixture : IAsyncLifetime
{
  private readonly MongoDbFixture _mongoDbFixture;
  public MongoDbCleanupFixture(MongoDbFixture mongoDbFixture)
  {
    _mongoDbFixture = mongoDbFixture ?? throw new ArgumentNullException(nameof(mongoDbFixture));
  }
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

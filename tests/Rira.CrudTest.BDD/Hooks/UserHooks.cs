using Rira.CrudTest.BDD.Initialization;
using TechTalk.SpecFlow;

namespace Rira.CrudTest.BDD.Hooks;

[Binding]
public class UserHooks
{
  private static MongoDbFixture _mongoFixture = null!;

  [BeforeTestRun(Order = 0)]
  public static async Task BeforeTestRun()
  {
    _mongoFixture = new MongoDbFixture();
    await _mongoFixture.InitializeAsync();
  }

  [BeforeScenario(Order = 1)]
  public static async Task CleanUpMongoDbFixture()
  {
    var entities = await _mongoFixture.userRepository!.GetAllAsync();
    foreach (var entity in entities!)
      await _mongoFixture.userRepository.RemoveAsync(entity.Id);
  }

  [BeforeScenario(Order = 2)]
  public static void RegisterMongoFixtureInScenarioContext(ScenarioContext context)
  {
    context.Set(_mongoFixture);
  }

  [AfterTestRun]
  public static void AfterTestRun()
  {
    _mongoFixture.DisposeAsync().GetAwaiter().GetResult();
  }
}

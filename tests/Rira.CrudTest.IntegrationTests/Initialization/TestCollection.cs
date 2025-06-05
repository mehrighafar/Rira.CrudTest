using Xunit;

namespace Rira.CrudTest.IntegrationTests.Initialization;

[CollectionDefinition("TestCollection")]
public class TestCollection : ICollectionFixture<MongoDbFixture>
{
}

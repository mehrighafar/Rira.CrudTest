using Xunit;

namespace Rira.CrudTest.FunctionalTests.Initialization;

[CollectionDefinition("MongoDbContainerCollection")]
public class MongoDbContainerCollection : ICollectionFixture<MongoDbFixture>
{
}

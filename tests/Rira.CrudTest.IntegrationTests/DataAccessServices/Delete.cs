using Rira.CrudTest.Infrastructure.Repositories;
using Rira.CrudTest.IntegrationTests.Initialization;
using Xunit;

namespace Rira.CrudTest.IntegrationTests.DataAccessServices;

[Collection(nameof(TestCollection))]
public class Delete(MongoDbFixture fixture) : IClassFixture<MongoDbCleanupFixture>
{
  private readonly MongoUserRepository _repository = fixture.userRepository!;

  [Fact]
  public async Task Deletes_Item_After_Adding_It()
  {
    // Arrange
    var init = new UserInitialization();
    init.CreateUsers();
    var User = init.testUserList[0];

    var addedUser = await _repository!.AddAsync(User);

    // Act
    await _repository.RemoveAsync(addedUser!.Id);

    //Assert
    var result = await _repository!.GetAllAsync();
    Assert.DoesNotContain((await _repository!.GetAllAsync())!,
      User => User.Id == addedUser.Id);
  }
}

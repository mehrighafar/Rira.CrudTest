using FluentAssertions;
using Rira.CrudTest.Infrastructure.Repositories;
using Rira.CrudTest.IntegrationTests.Initialization;
using Xunit;

namespace Rira.CrudTest.IntegrationTests.DataAccessServices;

[Collection(nameof(TestCollection))]
public class Update(MongoDbFixture fixture) : IClassFixture<MongoDbCleanupFixture>
{
  private readonly IUserRepository _repository = fixture.userRepository!;

  [Fact]
  public async Task Updates_Item_After_Adding_It()
  {
    // Arrange
    var init = new UserInitialization();
    init.CreateUsers();
    var User = init.testUserList[0];

    var addedUser = await _repository!.AddAsync(User);

    // fetch the item and update its FirstName
    var newUser = await _repository.GetByIdAsync(addedUser!.Id);

    if (newUser == null)
    {
      Assert.NotNull(newUser);
      return;
    }
    Assert.NotSame(addedUser, newUser);
    var newName = Guid.NewGuid().ToString();
    addedUser!.FirstName = newName;

    // Act
    await _repository.UpdateAsync(addedUser);

    var updatedItem = await _repository.GetByIdAsync(addedUser!.Id);

    // Assert
    Assert.NotNull(updatedItem);
    Assert.True(updatedItem!.FirstName == newName);
    addedUser.Should().BeEquivalentTo(updatedItem);
  }
}

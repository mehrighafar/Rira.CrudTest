using FluentAssertions;
using Rira.CrudTest.Core.UserAggregate.Entities;
using Rira.CrudTest.Infrastructure.Repositories;
using Rira.CrudTest.IntegrationTests.Initialization;
using MongoDB.Driver;
using Xunit;

namespace Rira.CrudTest.IntegrationTests.DataAccessServices;

[Collection(nameof(TestCollection))]
public class GetById(MongoDbFixture fixture) : IClassFixture<MongoDbCleanupFixture>
{
  private readonly IUserRepository _repository = fixture.userRepository!;

  [Fact]
  public async Task Adds_Users_And_Gets_By_Id()
  {
    // Arrange
    var init = new UserInitialization();
    init.CreateUsers();
    var userList = init.testUserList;
    IList<User> addedUserList = [];

    //Act
    foreach (var User in userList!)
      addedUserList.Add((await _repository!.AddAsync(User))!);

    var getUser = await _repository!.GetByIdAsync(addedUserList[0].Id);

    //Assert
    Assert.NotNull(getUser);
    getUser.Should().BeEquivalentTo(addedUserList[0]);
  }
}

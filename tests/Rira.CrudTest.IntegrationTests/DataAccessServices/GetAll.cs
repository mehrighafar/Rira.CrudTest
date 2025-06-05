using FluentAssertions;
using Rira.CrudTest.Core.UserAggregate.Entities;
using Rira.CrudTest.Infrastructure.Repositories;
using Rira.CrudTest.IntegrationTests.Initialization;
using MongoDB.Driver;
using Xunit;

namespace Rira.CrudTest.IntegrationTests.DataAccessServices;

[Collection(nameof(TestCollection))]
public class GetAll(MongoDbFixture fixture) : IClassFixture<MongoDbCleanupFixture>
{
  private readonly IUserRepository _repository = fixture.userRepository!;

  [Fact]
  public async Task Adds_Users_And_Gets_All()
  {
    // Arrange
    var init = new UserInitialization();
    init.CreateUsers();
    var userList = init.testUserList;
    IList<User> addedUserList = [];

    //Act
    foreach (var User in userList!)
      addedUserList.Add((await _repository!.AddAsync(User))!);

    var users = await _repository!.GetAllAsync();

    //Assert
    Assert.NotNull(users);
    users.Should().BeEquivalentTo(addedUserList);
  }
}

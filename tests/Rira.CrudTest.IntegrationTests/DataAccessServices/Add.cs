using FluentAssertions;
using MongoDB.Driver;
using Rira.CrudTest.Core.UserAggregate.Entities;
using Rira.CrudTest.Infrastructure.Repositories;
using Rira.CrudTest.IntegrationTests.Initialization;
using Xunit;

namespace Rira.CrudTest.IntegrationTests.DataAccessServices;

[Collection(nameof(TestCollection))]
public class Add(MongoDbFixture fixture) : IClassFixture<MongoDbCleanupFixture>
{
  private readonly IUserRepository _repository = fixture.userRepository!;

  [Fact]
  public async Task Adds_User_And_Sets_Id()
  {
    // Arrange
    var init = new UserInitialization();
    init.CreateUsers();
    var User = init.testUserList[0];
    User? newUser = null;

    //Act
    await _repository!.AddAsync(User);

    var users = await _repository!.GetAllAsync();
    if (users is not null)
      newUser = users!.FirstOrDefault();

    //Assert
    Assert.NotNull(newUser);
    newUser.Should().BeEquivalentTo(User);
  }
}

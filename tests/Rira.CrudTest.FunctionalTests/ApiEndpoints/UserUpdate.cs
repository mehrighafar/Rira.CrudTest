using System.Text.Json;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Rira.CrudTest.FunctionalTests.Initialization;
using Rira.CrudTest.Infrastructure.Repositories;
using Xunit;
using EntityUser = Rira.CrudTest.Core.UserAggregate.Entities.User;
using ProtoUser = Rira.CrudTest.FunctionalTests.Initialization.Grpc.Protos;

namespace Rira.CrudTest.FunctionalTests.ApiEndpoints;

[Collection(nameof(MongoDbContainerCollection))]
public class UserUpdate : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<MongoDbCleanupFixture>
{
  private readonly IUserRepository _userRepository;
  private readonly ProtoUser.Users.UsersClient? _client;
  private readonly List<EntityUser> _userList = [];

  public UserUpdate(CustomWebApplicationFactory<Program> factory, MongoDbFixture fixture)
  {
    _userRepository = fixture.userRepository!;

    var clientApiInitialization = new ClientApiInitialization(factory);
    _client = clientApiInitialization.client;
  }
  [Fact]
  public async Task Adds_And_Updates_User_FirstName()
  {
    // Arrange
    await AddAndGetUser();

    // Act
    var newName = new Guid().ToString();
    _userList[0]!.FirstName = newName;

    var updatedUser = new ProtoUser.UpdateUserRequest
    {
      Id = _userList[0].Id.ToString(),
      FirstName = newName,
      LastName = _userList[0].LastName,
      NationalCode = _userList[0].NationalCode,
      DateOfBirth = Timestamp.FromDateTime(_userList[0].DateOfBirth.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc))
    };

    var getUpdatedUser = await _client!.UpdateAsync(updatedUser);

    // Assert
    Assert.NotNull(getUpdatedUser.User);
    getUpdatedUser!.User.Id.Should().BeEquivalentTo(_userList[0].Id.ToString());
    getUpdatedUser!.User.FirstName.Should().BeEquivalentTo(_userList[0].FirstName);
    getUpdatedUser!.User.LastName.Should().BeEquivalentTo(_userList[0].LastName);
    getUpdatedUser!.User.NationalCode.Should().BeEquivalentTo(_userList[0].NationalCode);
    getUpdatedUser!.User.DateOfBirth.Should().BeEquivalentTo(
      Timestamp.FromDateTime(_userList[0].DateOfBirth
      .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)));
  }
  private async Task AddAndGetUser()
  {
    var init = new UserInitialization();
    init.CreateUsers();
    var user = init.testUserList[0];

    _userList.Add((await _userRepository!.AddAsync(user!))!);
  }

}


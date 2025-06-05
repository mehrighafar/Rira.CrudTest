using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Rira.CrudTest.FunctionalTests.Initialization;
using Rira.CrudTest.Infrastructure.Repositories;
using Xunit;
using EntityUser = Rira.CrudTest.Core.UserAggregate.Entities.User;
using ProtoUser = Rira.CrudTest.FunctionalTests.Initialization.Grpc.Protos;

namespace Rira.CrudTest.FunctionalTests.ApiEndpoints;

[Collection(nameof(MongoDbContainerCollection))]
public class UserDelete : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<MongoDbCleanupFixture>
{
  private readonly IUserRepository _userRepository;
  private readonly ProtoUser.Users.UsersClient? _client;
  private readonly List<EntityUser> _userList = [];

  public UserDelete(CustomWebApplicationFactory<Program> factory, MongoDbFixture fixture)
  {
    _userRepository = fixture.userRepository!;

    var clientApiInitialization = new ClientApiInitialization(factory);
    _client = clientApiInitialization.client;
  }
  [Fact]
  public async Task Adds_And_Deletes_User_And_Returns_204()
  {
    // Arrange
    await AddAndGetUser();

    // Act
    var deleteResponse = await _client!.DeleteAsync(new ProtoUser.UserIdRequest { Id = _userList[0]!.Id.ToString() });

    // Assert
    deleteResponse.Should().BeEquivalentTo(new Empty());
  }

  private async Task AddAndGetUser()
  {
    var init = new UserInitialization();
    init.CreateUsers();
    var User = init.testUserList[0];

    _userList.Add((await _userRepository!.AddAsync(User!))!);
  }
}


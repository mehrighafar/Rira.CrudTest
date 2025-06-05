using System.Text.Json;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Rira.CrudTest.FunctionalTests.Initialization;
using Rira.CrudTest.FunctionalTests.Initialization.Grpc.Protos;
using Rira.CrudTest.Infrastructure.Repositories;
using Xunit;
using EntityUser = Rira.CrudTest.Core.UserAggregate.Entities.User;

namespace Rira.CrudTest.FunctionalTests.ApiEndpoints;

[Collection(nameof(MongoDbContainerCollection))]
public class UserList : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<MongoDbCleanupFixture>
{
  private readonly IUserRepository _userRepository;
  private readonly Users.UsersClient? _client;
  private readonly List<EntityUser> _userList = [];

  public UserList(CustomWebApplicationFactory<Program> factory, MongoDbFixture fixture)
  {
    _userRepository = fixture.userRepository!;

    var clientApiInitialization = new ClientApiInitialization(factory);
    _client = clientApiInitialization.client;
  }
  [Fact]
  public async Task Adds_And_Returns_All_Users()
  {
    await AddUsers();

    var response = await _client!.GetAllAsync(new Empty());

    Assert.NotNull(response);
    Assert.NotEmpty(response!.Users);
    Assert.Equal(_userList.Count, response!.Users.Count());

    foreach (var user in response.Users)
    {
      var userEntity = _userList.FirstOrDefault(u => u.Id.ToString() == user.Id);
      Assert.NotNull(userEntity);
      user.Id.Should().BeEquivalentTo(userEntity!.Id.ToString());
      user.FirstName.Should().BeEquivalentTo(userEntity.FirstName);
      user.LastName.Should().BeEquivalentTo(userEntity.LastName);
      user.NationalCode.Should().BeEquivalentTo(userEntity.NationalCode);
      user.DateOfBirth.Should().BeEquivalentTo(
        Timestamp.FromDateTime(userEntity.DateOfBirth.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)));
    }
  }
  private async Task AddUsers()
  {
    var init = new UserInitialization();
    init.CreateUsers();
    var userList = init.testUserList;

    foreach (var User in userList!)
      _userList.Add((await _userRepository!.AddAsync(User))!);
  }
}


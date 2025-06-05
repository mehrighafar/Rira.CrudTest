using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Rira.CrudTest.FunctionalTests.Initialization;
using Rira.CrudTest.Infrastructure.Repositories;
using Xunit;
using EntityUser = Rira.CrudTest.Core.UserAggregate.Entities.User;
using ProtoUser = Rira.CrudTest.FunctionalTests.Initialization.Grpc.Protos;

namespace Rira.CrudTest.FunctionalTests.ApiEndpoints;

[Collection(nameof(MongoDbContainerCollection))]
public class UserGetById : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<MongoDbCleanupFixture>
{
  private readonly IUserRepository _userRepository;
  private readonly ProtoUser.Users.UsersClient? _client;
  private readonly List<EntityUser> _userList = [];

  public UserGetById(CustomWebApplicationFactory<Program> factory, MongoDbFixture fixture)
  {
    _userRepository = fixture.userRepository!;

    var clientApiInitialization = new ClientApiInitialization(factory);
    _client = clientApiInitialization.client;
  }
  [Fact]
  public async Task Returns_Added_User_Given_Id()
  {
    await AddUser();

    var response = await _client!.GetByIdAsync(
      new ProtoUser.UserIdRequest { Id = _userList[0].Id.ToString() }
      );

    Assert.NotNull(response.User);
    response!.User.Id.Should().BeEquivalentTo(_userList[0].Id.ToString());
    response!.User.FirstName.Should().BeEquivalentTo(_userList[0].FirstName);
    response!.User.LastName.Should().BeEquivalentTo(_userList[0].LastName);
    response!.User.NationalCode.Should().BeEquivalentTo(_userList[0].NationalCode);
    response!.User.DateOfBirth.Should().BeEquivalentTo(
      Timestamp.FromDateTime(_userList[0].DateOfBirth
      .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)));
  }

  [Fact]
  public async Task Returns_Not_Found_Given_Random_Id()
  {
    // Arrange
    var randomId = new Guid().ToString();
    // Act
    var ex = await Assert.ThrowsAsync<RpcException>(async ()
        => await _client!.GetByIdAsync(new ProtoUser.UserIdRequest { Id = randomId }));

    // Assert  
    ex.Should().NotBeNull();
    ex.StatusCode.Should().Be(StatusCode.NotFound);
  }


  private async Task AddUser()
  {
    var init = new UserInitialization();
    init.CreateUsers();
    var User = init.testUserList[0];

    _userList.Add((await _userRepository!.AddAsync(User!))!);
  }
}

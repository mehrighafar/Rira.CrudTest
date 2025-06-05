using System.Text.Json;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Rira.CrudTest.FunctionalTests.Initialization;
using Rira.CrudTest.FunctionalTests.Initialization.Grpc.Protos;
using Rira.CrudTest.Infrastructure.Repositories;
using Xunit;

namespace Rira.CrudTest.FunctionalTests.ApiEndpoints;

[Collection(nameof(MongoDbContainerCollection))]
public class UserAdd : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<MongoDbCleanupFixture>
{
  private readonly IUserRepository _userRepository;
  private readonly Users.UsersClient? _client;

  public UserAdd(CustomWebApplicationFactory<Program> factory, MongoDbFixture fixture)
  {
    _userRepository = fixture.userRepository!;
    var clientApiInitialization = new ClientApiInitialization(factory);
    _client = clientApiInitialization.client;
  }
  [Fact]
  public async Task Adds_And_Returns_User()
  {
    var user = GetCreateUserRequest();
    var response = await _client!.AddAsync(user);

    var getUserResult = await GetUser(Guid.Parse(response!.User.Id));

    Assert.NotNull(response.User);
    response!.User.Id.Should().BeEquivalentTo(getUserResult!.Id.ToString());
    response!.User.FirstName.Should().BeEquivalentTo(getUserResult!.FirstName);
    response!.User.LastName.Should().BeEquivalentTo(getUserResult!.LastName);
    response!.User.NationalCode.Should().BeEquivalentTo(getUserResult!.NationalCode);
    response!.User.DateOfBirth.Should().BeEquivalentTo(
      Timestamp.FromDateTime(getUserResult!.DateOfBirth
      .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)));
  }
  private CreateUserRequest GetCreateUserRequest()
  {
    var init = new UserInitialization();
    init.CreateCreateUserRequests();
    return init.createUserRequestTest;
  }

  private async Task<Core.UserAggregate.Entities.User?> GetUser(Guid id)
  {
    return await _userRepository!.GetByIdAsync(id);
  }
}


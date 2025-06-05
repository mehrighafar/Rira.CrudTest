using dotenv.net;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Rira.CrudTest.BDD.Initialization;
using Rira.CrudTest.BDD.Initialization.Grpc.Protos;
using Rira.CrudTest.Infrastructure.Repositories;
using TechTalk.SpecFlow;

namespace Rira.CrudTest.BDD.Steps;

[Binding]
public class UserWebApiStepDefinitions
{
  private readonly CustomWebApplicationFactory<Program> _factory;
  private readonly IUserRepository _userRepository;
  private readonly ScenarioContext _scenarioContext;
  private CreateUserRequest? _createUserRequest;
  private static string? _baseAddress;
  private GrpcChannel? _channel;
  private Users.UsersClient? _client;

  public UserWebApiStepDefinitions(
    CustomWebApplicationFactory<Program> factory
    , ScenarioContext scenarioContext)
  {
    _scenarioContext = scenarioContext;
    _factory = factory;

    var mongoFixture = _scenarioContext.Get<MongoDbFixture>();
    _userRepository = mongoFixture.userRepository!;

    DotEnv.Load(new DotEnvOptions(envFilePaths: ["../../../.env"]));

    // Grpc client
    _baseAddress = Environment.GetEnvironmentVariable("API_BASE_ADDRESS")!
      ?? throw new InvalidOperationException("Api address for test is not set.");
  }

  [Given(@"I am a client")]
  public void GivenIAmAClient()
  {
    var httpHandler = _factory.Server.CreateHandler();
    _channel = GrpcChannel.ForAddress(_baseAddress!, new GrpcChannelOptions
    {
      HttpHandler = httpHandler
    });

    _client = new Users.UsersClient(_channel);
  }

  [Given(@"The repository is seeded with random users")]
  public async Task GivenTheRepositoryIsSeededWithRandomUsers()
  {
    var init = new UserInitialization();
    init.CreateUsers();
    var users = init.testUserList;

    foreach (var user in users!)
      await _userRepository.AddAsync(user);

    _scenarioContext["SeededUsers"] = users;
  }

  [Given(@"I have generated a random user")]
  public void GivenIHaveGeneratedARandomUser()
  {
    var init = new UserInitialization();
    init.CreateCreateUserRequests();
    _createUserRequest = init.createUserRequestTest;
    _scenarioContext["NewUser"] = _createUserRequest;
  }

  /// <summary>
  /// Scenario 1
  /// </summary>
  [When(@"I make a GET request to user")]
  public async Task WhenIMakeAGetRequestTo()
  {
    var response = await _client!.GetAllAsync(new Empty());
    _scenarioContext["GetUsersResponse"] = response;
  }

  [Then(@"The response should contain the seeded users")]
  public void ThenTheResponseShouldContainTheSeededUsers()
  {
    var expectedUsers = _scenarioContext.Get<List<Core.UserAggregate.Entities.User>>("SeededUsers");
    var actualUsers = _scenarioContext.Get<UserListResponse>("GetUsersResponse").Users;

    foreach (var expected in expectedUsers)
    {
      _ = actualUsers.Should().ContainEquivalentOf(new
      {
        Id = expected.Id.ToString(),
        expected.FirstName,
        expected.LastName,
        expected.NationalCode,
        DateOfBirth = Timestamp.FromDateTime(
          expected.DateOfBirth
          .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc))
      });
    }
  }

  /// <summary>
  /// Scenario 2
  /// </summary>
  [When(@"I make a POST request with the user to user")]
  public async Task WhenIMakeAPostRequestWithUserTo()
  {
    var user = _scenarioContext.Get<CreateUserRequest>("NewUser");
    var response = await _client!.AddAsync(user);
    _scenarioContext["CreatedUserResponse"] = response;
  }

  [Then(@"The response should match the sent user")]
  public void ThenTheResponseShouldMatchTheSentUser()
  {
    var expected = _scenarioContext.Get<CreateUserRequest>("NewUser");
    var actual = _scenarioContext.Get<UserResponse>("CreatedUserResponse").User;

    actual.FirstName.Should().Be(expected.FirstName);
    actual.LastName.Should().Be(expected.LastName);
    actual.NationalCode.Should().Be(expected.NationalCode);
    actual.DateOfBirth.Should().Be(expected.DateOfBirth);
  }

  /// <summary>
  /// Scenario 3
  /// </summary>
  [When(@"I make a PUT request with updated data to user")]
  public async Task WhenIMakeAPutRequestWithUpdatedDataTo()
  {
    var userList = await _client!.GetAllAsync(new Empty());
    var user = userList.Users.First();

    var updatedRequest = new UpdateUserRequest
    {
      Id = user.Id,
      FirstName = "UpdatedFirst",
      LastName = "UpdatedLast",
      NationalCode = user.NationalCode,
      DateOfBirth = user.DateOfBirth
    };

    var response = await _client.UpdateAsync(updatedRequest);
    _scenarioContext["UpdatedUserRequest"] = updatedRequest;
    _scenarioContext["UpdateUserResponse"] = response;
  }

  [Then(@"The response should match the updated user")]
  public void ThenTheResponseShouldMatchTheUpdatedUser()
  {
    var expected = _scenarioContext.Get<UpdateUserRequest>("UpdatedUserRequest");
    var actual = _scenarioContext.Get<UserResponse>("UpdateUserResponse").User;

    actual.Id.Should().Be(expected.Id);
    actual.FirstName.Should().Be(expected.FirstName);
    actual.LastName.Should().Be(expected.LastName);
    actual.NationalCode.Should().Be(expected.NationalCode);
  }

  /// <summary>
  /// Scenario 4
  /// </summary>
  [When(@"I make a DELETE request for that user to user")]
  public async Task WhenIMakeADeleteRequestForThatUserTo()
  {
    var users = await _client!.GetAllAsync(new Empty());
    var userToDelete = users.Users.First();

    var response = await _client.DeleteAsync(new UserIdRequest { Id = userToDelete.Id });
    _scenarioContext["DeleteUserResponse"] = response;
  }

  [Then(@"The response should be empty")]
  public void ThenTheResponseShouldBeEmpty()
  {
    var actual = _scenarioContext.Get<Empty>("DeleteUserResponse");

    actual.Should().BeEquivalentTo(new Empty());
  }
}

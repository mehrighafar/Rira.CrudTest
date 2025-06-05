using System.Text.Json;
using dotenv.net;
using Grpc.Net.Client;
using Rira.CrudTest.FunctionalTests.Initialization.Grpc.Protos;
using Xunit;

namespace Rira.CrudTest.FunctionalTests.Initialization;

[Collection(nameof(MongoDbContainerCollection))]
public class ClientApiInitialization : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private static string? _baseAddress;
  private readonly GrpcChannel _channel;
  public readonly Users.UsersClient client;

  public ClientApiInitialization(CustomWebApplicationFactory<Program> factory)
  {
    DotEnv.Load(new DotEnvOptions(envFilePaths: ["../../../.env"]));

    // Grpc client
    _baseAddress = Environment.GetEnvironmentVariable("API_BASE_ADDRESS")!
      ?? throw new InvalidOperationException("Api address for test is not set.");

    var httpHandler = factory.Server.CreateHandler();
    _channel = GrpcChannel.ForAddress(_baseAddress, new GrpcChannelOptions
    {
      HttpHandler = httpHandler
    });

    client = new Users.UsersClient(_channel);
  }
}

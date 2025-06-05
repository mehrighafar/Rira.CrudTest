using dotenv.net;
using FluentValidation;
using Grpc.Core;
using MediatR;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.Infrastructure.DataAccessServices;
using Rira.CrudTest.Infrastructure.Extensions;
using Rira.CrudTest.UseCases;
using Rira.CrudTest.UseCases.PipelineBehaviors;
using Rira.CrudTest.Web.Configurations;
using Rira.CrudTest.Web.Grpc.GrpcServices;
using Rira.CrudTest.Web.GrpcInterceptor;
using Rira.CrudTest.Web.MapperProfiles;
using Rira.CrudTest.Web.Middleware;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();

const string serviceName = "Users";
const string serviceVersion = "1.0.0";
var instrumentation = new Instrumentation();
var activitySourceName = instrumentation.ActivitySource.Name;
var collectorEndpoint = Environment.GetEnvironmentVariable("COLLECTOR") ??
  throw new RpcException(new Status(StatusCode.FailedPrecondition, "COLLECTOR endpoint is not set."));
var lokiEndpoint = Environment.GetEnvironmentVariable("LOKI") ??
  throw new RpcException(new Status(StatusCode.FailedPrecondition, "LOKI endpoint is not set."));

// Add services to the container.
builder.Services.AddGrpc(options =>
{
  options.EnableDetailedErrors = true;
  options.MaxReceiveMessageSize = null; // 4 MB
  options.MaxSendMessageSize = 5 * 1024 * 1024; // 5 MB
  options.Interceptors.Add<GrpcExceptionInterceptor>();
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// DI
builder.Services.AddAutoMapper(cfg =>
{
  cfg.AddProfile(new EntityUserAndGrpcCreateUserRequestMapperProfile());
  cfg.AddProfile(new EntityUserAndGrpcUpdateUserRequestMapperProfile());
  cfg.AddProfile(new EntityUserANDGrpcUserMapperProfile());
}, typeof(AssemblyReference).Assembly);

builder.Services.AddTransient<IUserDataAccessService, UserDataAccessService>();
builder.Services.AddSingleton<Instrumentation>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<OpenTelemetryContextMiddleware>();

if (builder.Environment.EnvironmentName != "Test")
{
  // Register database   
  builder.Services.AddMongoDatabase();
}
builder.Services.AddRepository();

// OpenTelemetry
builder.AddOpentTelemetryConfigs(activitySourceName, serviceName, serviceVersion, collectorEndpoint);

builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("Version", serviceVersion)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(logger);

builder.AddLoggerConfigs();

logger.Information("Starting web host");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseHsts();
}

app.MapGrpcService<UserService>();
app.UseHttpsRedirection();

app.UseRouting();
app.MapControllers();
app.UseMiddleware<OpenTelemetryContextMiddleware>();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();


public partial class Program { }

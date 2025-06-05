﻿using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Rira.CrudTest.Infrastructure.Extensions;

public static class MongoDbExtension
{
  private static bool _serializersRegistered = false;

  public static IServiceCollection AddMongoDatabase(this IServiceCollection services)
  {
    if (!_serializersRegistered)
    {
      BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
      _serializersRegistered = true;
    }

    var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
                ?? throw new RpcException(new Status(StatusCode.FailedPrecondition, "Connection string is not set.")));
    var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
        ?? throw new RpcException(new Status(StatusCode.FailedPrecondition, "DatabasenName is not set.")));
    services.AddScoped(provider => mongoDatabase);
    return services;
  }
}

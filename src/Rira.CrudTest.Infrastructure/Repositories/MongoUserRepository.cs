using System.Diagnostics;
using Grpc.Core;
using MongoDB.Driver;
using Rira.CrudTest.Core.Exceptions;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.Infrastructure.Repositories;

public class MongoUserRepository(IMongoDatabase mongoDatabase) : IUserRepository
{
  private readonly IMongoCollection<User> _mongoCollection
    = mongoDatabase.GetCollection<User>(nameof(User))
    ?? throw new RpcException(new Status(StatusCode.FailedPrecondition, "Mongodb is not set."));

  public async Task<IEnumerable<User>?> GetAllAsync()
  {
    var filter = Builders<User>.Filter.Empty;
    return await _mongoCollection.Find(filter).ToListAsync();
  }

  public async Task<User?> GetByIdAsync(Guid id)
  {
    using var activity = Activity.Current?.Source.StartActivity("MongoDB GetById");
    activity?.SetTag($"{nameof(User)}.{nameof(User.Id)}", id.ToString());

    var result = await _mongoCollection.Find(e => e.Id == id).FirstOrDefaultAsync();

    if (result == null)
    {
      activity?.SetStatus(ActivityStatusCode.Error, "User Not Found");
      throw new UserNotFoundException(id);
    }

    return result;
  }

  public async Task<User?> AddAsync(User entity)
  {
    using var activity = Activity.Current?.Source.StartActivity("MongoDB AddUser");
    activity?.SetTag($"{nameof(User)}.{nameof(User.Id)}", entity.Id.ToString());
    activity?.SetTag($"{nameof(User)}.{nameof(User.NationalCode)}", entity.NationalCode);

    if (!await IsNationalCodeUniqueInDb(entity.Id, entity.NationalCode))
    {
      activity?.SetStatus(ActivityStatusCode.Error, "Duplicate national code");
      throw new UserNotUniqueNationalCodeException(entity.NationalCode);
    }

    entity.Id = Guid.NewGuid();
    await _mongoCollection.InsertOneAsync(entity);
    return entity;
  }

  public async Task<User?> UpdateAsync(User entity)
  {
    using var activity = Activity.Current?.Source.StartActivity("MongoDB UpdateUser");
    activity?.SetTag($"{nameof(User)}.{nameof(User.Id)}", entity.Id.ToString());
    activity?.SetTag($"{nameof(User)}.{nameof(User.NationalCode)}", entity.NationalCode);

    if (!await IsNationalCodeUniqueInDb(entity.Id, entity.NationalCode))
    {
      activity?.SetStatus(ActivityStatusCode.Error, "Duplicate national code");
      throw new UserNotUniqueNationalCodeException(entity.NationalCode);
    }

    return await _mongoCollection.FindOneAndReplaceAsync
      (e => e.Id == entity.Id, entity, new FindOneAndReplaceOptions<User>
      {
        ReturnDocument = ReturnDocument.After
      });
  }

  public async Task<DeleteResult> RemoveAsync(Guid id)
  {
    using var activity = Activity.Current?.Source.StartActivity("MongoDB Remove");
    activity?.SetTag($"{nameof(User)}.{nameof(User.Id)}", id.ToString());

    return await _mongoCollection.DeleteOneAsync(e => e.Id == id);
  }

  private async Task<bool> IsNationalCodeUniqueInDb(Guid id, string nationalCode)
  {
    var filter = Builders<User>.Filter.And(
        Builders<User>.Filter.Ne(u => u.Id, id),
        Builders<User>.Filter.Eq(u => u.NationalCode, nationalCode));

    var existing = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    return existing == null;
  }
}

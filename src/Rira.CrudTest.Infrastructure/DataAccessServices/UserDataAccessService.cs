using System.Diagnostics;
using Grpc.Core;
using MongoDB.Driver;
using Rira.CrudTest.Core.Exceptions;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.Core.UserAggregate.Entities;
using Rira.CrudTest.Infrastructure.Repositories;

namespace Rira.CrudTest.Infrastructure.DataAccessServices;

public class UserDataAccessService(IUserRepository userRepository)
  : IUserDataAccessService
{
  private readonly IUserRepository _userRepository = userRepository;
  public async Task<IEnumerable<User>?> GetAllAsync()
  {
    var result = await _userRepository.GetAllAsync();
    if (result is null || !result.Any()) { return null; }

    return result;
  }

  public async Task<User?> GetByIdAsync(Guid id)
  {
    using var activity = Activity.Current?.Source.StartActivity("GetUserById");
    activity?.SetTag($"{nameof(User)}.{nameof(User.Id)}", id.ToString());

    var result = await _userRepository.GetByIdAsync(id);

    if (result is null)
    {
      activity?.AddEvent(new ActivityEvent("UserNotFound"));
      throw new UserNotFoundException(id);
    }

    return result;
  }
  public async Task<User?> AddAsync(User model)
  {
    using var activity = Activity.Current?.Source.StartActivity("AddUser", ActivityKind.Internal);
    activity?.SetTag($"{nameof(User)}.{nameof(User.Id)}", model.Id.ToString());
    activity?.SetTag($"{nameof(User)}.{nameof(User.NationalCode)}", model.NationalCode);

    var result = await _userRepository.AddAsync(model);

    return result;
  }
  public async Task<User?> UpdateAsync(User model)
  {
    using var activity = Activity.Current?.Source.StartActivity("UpdateUser");
    activity?.SetTag($"{nameof(User)}.{nameof(User.Id)}", model.Id.ToString());
    activity?.SetTag($"{nameof(User)}.{nameof(User.NationalCode)}", model.NationalCode);

    var result = await _userRepository.UpdateAsync(model);

    return result;
  }
  public async Task<bool> DeleteAsync(Guid id)
  {
    using var activity = Activity.Current?.Source.StartActivity("DeleteUser", ActivityKind.Internal);
    activity?.SetTag($"{nameof(User)}.{nameof(User.Id)}", id.ToString());

    var result = await _userRepository.RemoveAsync(id);

    return result.IsAcknowledged ?
    result.IsAcknowledged
    : throw new RpcException(new Status(StatusCode.Internal, $"An error occured while removing User: {id}."));
  }


}

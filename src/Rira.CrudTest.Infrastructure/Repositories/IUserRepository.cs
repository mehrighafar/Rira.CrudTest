using MongoDB.Driver;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.Infrastructure.Repositories;

public interface IUserRepository
{
  Task<IEnumerable<User>?> GetAllAsync();
  Task<User?> GetByIdAsync(Guid id);
  Task<User?> AddAsync(User entity);
  Task<User?> UpdateAsync(User entity);
  Task<DeleteResult> RemoveAsync(Guid id);
}

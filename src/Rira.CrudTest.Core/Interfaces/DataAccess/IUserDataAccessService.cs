using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.Core.Interfaces.DataAccess;

public interface IUserDataAccessService
{
  public Task<IEnumerable<User>?> GetAllAsync();
  public Task<User?> GetByIdAsync(Guid id);
  public Task<User?> AddAsync(User model);
  public Task<User?> UpdateAsync(User model);
  public Task<bool> DeleteAsync(Guid id);
}


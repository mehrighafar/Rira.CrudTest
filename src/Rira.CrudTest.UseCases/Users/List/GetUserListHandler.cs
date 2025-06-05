using MediatR;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.UseCases.Users.List;

public class GetUserListHandler(IUserDataAccessService userDataAccessService) : IRequestHandler<GetUserListQuery, IEnumerable<User>?>
{
  private readonly IUserDataAccessService _userDataAccessService = userDataAccessService;
  public async Task<IEnumerable<User>?> Handle(GetUserListQuery request, CancellationToken cancellationToken)
  {
    return await _userDataAccessService.GetAllAsync();
  }
}

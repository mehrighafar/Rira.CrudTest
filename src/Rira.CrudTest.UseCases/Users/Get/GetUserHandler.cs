using System.Diagnostics;
using Grpc.Core;
using MediatR;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.UseCases.Users.Get;

public class GetUserHandler(IUserDataAccessService userDataAccessService) : IRequestHandler<GetUserQuery, User>
{
  private readonly IUserDataAccessService _userDataAccessService = userDataAccessService;

  public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
  {
    Activity.Current?.AddTag("Get in Handle UserId", request.Id.ToString());

    var result = await _userDataAccessService.GetByIdAsync(Guid.Parse(request.Id));

    return result!;

  }
}


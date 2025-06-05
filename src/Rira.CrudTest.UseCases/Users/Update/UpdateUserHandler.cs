using System.Diagnostics;
using MediatR;
using Rira.CrudTest.Core.UserAggregate.Entities;
using Rira.CrudTest.Core.Interfaces.DataAccess;

namespace Rira.CrudTest.UseCases.Users.Update;

public class UpdateUserHandler(IUserDataAccessService userDataAccessService) : IRequestHandler<UpdateUserCommand, User>
{
  private readonly IUserDataAccessService _userDataAccessService = userDataAccessService;

  public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
  {
    Activity.Current?.AddTag("Update in Handle UserId", request.NewModel.Id.ToString());

    return (await _userDataAccessService.UpdateAsync(request.NewModel))!;

  }
}

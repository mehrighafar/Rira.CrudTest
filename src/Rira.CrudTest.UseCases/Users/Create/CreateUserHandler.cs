using System.Diagnostics;
using MediatR;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.UseCases.Users.Create;

public class CreateUserHandler(IUserDataAccessService userDataAccessService) : IRequestHandler<CreateUserCommand, User>
{
  private readonly IUserDataAccessService _userDataAccessService = userDataAccessService;

  public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {
    Activity.Current?.AddTag("Create in Handle UserId", request.NewModel.Id.ToString());

    var result = await _userDataAccessService.AddAsync(request.NewModel);
    return result!;
  }
}


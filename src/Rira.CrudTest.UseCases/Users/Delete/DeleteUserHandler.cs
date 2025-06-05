using System.Diagnostics;
using MediatR;
using Rira.CrudTest.Core.Interfaces.DataAccess;

namespace Rira.CrudTest.UseCases.Users.Delete;

public class DeleteUserHandler(IUserDataAccessService userDataAccessService) : IRequestHandler<DeleteUserCommand, bool>
{
  private readonly IUserDataAccessService _userDataAccessService = userDataAccessService;
  public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
  {
    Activity.Current?.AddTag("Delete in Handle UserId", request.Id);

    var result = await _userDataAccessService.DeleteAsync(Guid.Parse(request.Id));

    return result;
  }
}


using MediatR;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.UseCases.Users.List;

public class GetUserListQuery() : IRequest<IEnumerable<User>>;

using MediatR;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.UseCases.Users.Update;

public record UpdateUserCommand(User NewModel) : IRequest<User>;

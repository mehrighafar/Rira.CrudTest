using MediatR;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.UseCases.Users.Create;

public record CreateUserCommand(User NewModel) : IRequest<User>;

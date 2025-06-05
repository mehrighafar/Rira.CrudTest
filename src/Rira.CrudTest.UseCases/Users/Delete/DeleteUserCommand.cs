using MediatR;

namespace Rira.CrudTest.UseCases.Users.Delete;

public record DeleteUserCommand(string Id) : IRequest<bool>;

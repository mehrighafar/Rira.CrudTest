using MediatR;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.UseCases.Users.Get;

public record GetUserQuery(string Id) : IRequest<User>;

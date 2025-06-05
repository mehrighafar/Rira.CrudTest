using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Rira.CrudTest.UseCases.Users.Create;
using Rira.CrudTest.UseCases.Users.Delete;
using Rira.CrudTest.UseCases.Users.Get;
using Rira.CrudTest.UseCases.Users.List;
using Rira.CrudTest.UseCases.Users.Update;
using Rira.CrudTest.Web.Grpc.Protos;

namespace Rira.CrudTest.Web.Grpc.GrpcServices;

public class UserService(IMediator mediator, IMapper mapper) : Users.UsersBase
{
  private readonly IMediator _mediator = mediator;
  private readonly IMapper _mapper = mapper;

  public override async Task<UserResponse> Add(CreateUserRequest request, ServerCallContext context)
  {
    var x = _mapper.Map<Core.UserAggregate.Entities.User>(request);
    var result = await _mediator.Send(
      new CreateUserCommand(x));

    var resultMapped = _mapper.Map<User>(result);

    return new UserResponse { User = resultMapped };
  }

  public override async Task<UserResponse> GetById(UserIdRequest request, ServerCallContext context)
  {

    var result = await _mediator.Send(new GetUserQuery(request.Id));

    var resultMapped = _mapper.Map<User>(result);

    return new UserResponse { User = resultMapped };
  }

  public override async Task<UserResponse> Update(UpdateUserRequest request, ServerCallContext context)
  {
    var result = await _mediator.Send(new UpdateUserCommand(
      _mapper.Map<Core.UserAggregate.Entities.User>(request)
      ));

    var resultMapped = _mapper.Map<User>(result);

    return new UserResponse { User = resultMapped };
  }

  public override async Task<Empty> Delete(UserIdRequest request, ServerCallContext context)
  {
    await _mediator.Send(new DeleteUserCommand(request.Id));
    return new Empty();
  }

  public override async Task<UserListResponse> GetAll(Empty request, ServerCallContext context)
  {
    var result = await _mediator.Send(new GetUserListQuery());
    if (result is null || !result.Any())
      return new UserListResponse();

    var resultMapped = _mapper.Map<IEnumerable<User>>(result);

    var response = new UserListResponse();
    response.Users.AddRange(resultMapped!);

    return response;
  }
}

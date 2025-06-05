using AutoFixture;
using Google.Protobuf.WellKnownTypes;
using Rira.CrudTest.BDD.Initialization.Grpc.Protos;
using EntityUser = Rira.CrudTest.Core.UserAggregate.Entities.User;

namespace Rira.CrudTest.BDD.Initialization;
public class UserInitialization
{
  public CreateUserRequest createUserRequestTest = new();
  public IList<EntityUser> testUserList = [];
  private static readonly string[] _nationalCodes =
    [
      "0388333839", "3625527545", "3453786327", "3730003003", "0939685736"
    ];

  public void CreateCreateUserRequests()
  {
    var fixture = new Fixture();
    createUserRequestTest = fixture.Build<CreateUserRequest>()
            .With(u => u.FirstName, "First")
            .With(u => u.LastName, "Last")
            .With(u => u.NationalCode, _nationalCodes[0])
            .With(u => u.DateOfBirth, Timestamp.FromDateTime
            (new DateOnly(1991, 1, 1)
            .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)))
            .Create();
  }

  public void CreateUsers()
  {
    var fixture = new Fixture();
    testUserList = Enumerable.Range(1, 5)
        .Select(i => fixture.Build<EntityUser>()
            .With(u => u.FirstName, $"First{i}")
            .With(u => u.LastName, $"Last{i}")
            .With(u => u.NationalCode, _nationalCodes[i - 1])
            .With(u => u.DateOfBirth, new DateOnly(1990 + i, 1, 1))
            .Create())
        .ToList();
  }
}

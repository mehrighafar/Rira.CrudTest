using AutoFixture;
using Rira.CrudTest.Core.UserAggregate.Entities;

namespace Rira.CrudTest.UnitTests.Initialization;
public class UserInitialization
{
  private readonly List<string> _nationalCodes = [];
  public IList<User> testUserList = [];
  private static readonly string[] collection = new[]
    {
      "0388333839", "3625527545", "3453786327", "3730003003", "0939685736"
    };

  public UserInitialization()
  {
    _nationalCodes.AddRange(collection);
  }

  public void CreateUsers()
  {
    var fixture = new Fixture();
    testUserList = Enumerable.Range(1, 5)
        .Select(i => fixture.Build<User>()
            .With(u => u.FirstName, $"First{i}")
            .With(u => u.LastName, $"Last{i}")
            .With(u => u.NationalCode, _nationalCodes[i - 1])
            .With(u => u.DateOfBirth, new DateOnly(1990 + i, 1, 1))
            .Create())
        .ToList();
  }
}

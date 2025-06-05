using Rira.CrudTest.Core.UserAggregate.Entities;
using Xunit;

namespace Rira.CrudTest.UnitTests.Core.UserAggregate;

public class UserConstructor
{
  private readonly string _testName = "test name";
  private User? _testUser;

  private User CreateUser()
  {
    return new User { FirstName = _testName };
  }

  [Fact]
  public void InitializesName()
  {
    _testUser = CreateUser();

    Assert.Equal(_testName, _testUser.FirstName);
  }
}

using FluentAssertions;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.UnitTests.Initialization;
using Rira.CrudTest.UseCases.Users.List;
using Moq;
using Xunit;

namespace Rira.CrudTest.UnitTests.UseCases.Users;

public class GetListUserHandlerHandle
{
  private readonly Mock<IUserDataAccessService> _userDataAccessServiceMock = new();
  private readonly GetUserListHandler _handler;

  public GetListUserHandlerHandle()
  {
    _handler = new GetUserListHandler(_userDataAccessServiceMock.Object);
  }

  [Fact]
  public async Task Returns_Successful_Given_Users()
  {
    // Arrange
    var init = new UserInitialization();
    init.CreateUsers();
    var users = init.testUserList;

    _userDataAccessServiceMock
        .Setup(service => service.GetAllAsync())
        .ReturnsAsync(users);

    //Act
    var results = await _handler.Handle(new GetUserListQuery(), CancellationToken.None);

    //Assert
    Assert.NotNull(results);
    Assert.Equal(users!.Count, results!.Count());
    users!.Should().BeEquivalentTo(results);
  }
}

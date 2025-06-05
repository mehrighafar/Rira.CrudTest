using FluentAssertions;
using Moq;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.UnitTests.Initialization;
using Rira.CrudTest.UseCases.Users.Get;
using Xunit;

namespace Rira.CrudTest.UnitTests.UseCases.Users;

public class GetUserHandlerHandle
{
  private readonly Mock<IUserDataAccessService> _userDataAccessServiceMock = new();
  private readonly GetUserHandler _handler;

  public GetUserHandlerHandle()
  {
    _handler = new GetUserHandler(_userDataAccessServiceMock.Object);
  }

  [Fact]
  public async Task Returns_Successful_Given_User()
  {
    // Arrange
    var init = new UserInitialization();
    init.CreateUsers();
    var User = init.testUserList[0];

    _userDataAccessServiceMock
        .Setup(service => service.GetByIdAsync(It.IsAny<Guid>()))
        .ReturnsAsync(User);

    //Act
    var result = await _handler.Handle(new GetUserQuery(User.Id.ToString()), CancellationToken.None);

    //Assert
    Assert.NotNull(result);
    User.Should().BeEquivalentTo(result);
  }
}

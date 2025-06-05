using FluentAssertions;
using Moq;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.Core.UserAggregate.Entities;
using Rira.CrudTest.UnitTests.Initialization;
using Rira.CrudTest.UseCases.Users.Create;
using Xunit;

namespace Rira.CrudTest.UnitTests.UseCases.Users;

public class CreateUserHandlerHandle
{
  private readonly Mock<IUserDataAccessService> _userDataAccessServiceMock = new();
  private readonly CreateUserHandler _handler;

  public CreateUserHandlerHandle()
  {
    _handler = new CreateUserHandler(_userDataAccessServiceMock.Object);
  }

  [Fact]
  public async Task Returns_Success_Given_User()
  {
    // Arrange
    var init = new UserInitialization();
    init.CreateUsers();
    var User = init.testUserList[0];

    _userDataAccessServiceMock
        .Setup(service => service.AddAsync(It.IsAny<User>()))
        .ReturnsAsync(User);

    //Act
    var result = await _handler.Handle(new CreateUserCommand(User), CancellationToken.None);

    //Assert
    Assert.NotNull(result);
    User.Should().BeEquivalentTo(result);
  }
}

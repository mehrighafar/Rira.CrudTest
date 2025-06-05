using FluentAssertions;
using Moq;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.Core.UserAggregate.Entities;
using Rira.CrudTest.UnitTests.Initialization;
using Rira.CrudTest.UseCases.Users.Update;
using Xunit;

namespace Rira.CrudTest.UnitTests.UseCases.Users;

public class UpdateUserHandlerHandle
{
  private readonly Mock<IUserDataAccessService> _userDataAccessServiceMock = new();
  private readonly UpdateUserHandler _handler;

  public UpdateUserHandlerHandle()
  {
    _handler = new UpdateUserHandler(_userDataAccessServiceMock.Object);
  }

  [Fact]
  public async Task Updates_Successful_Given_User()
  {
    // Arrange
    var newName = Guid.NewGuid().ToString();
    var init = new UserInitialization();
    init.CreateUsers();
    var User = init.testUserList[0];
    User.Id = Guid.NewGuid();

    User.FirstName = newName;

    _userDataAccessServiceMock
        .Setup(service => service.UpdateAsync(It.IsAny<User>()))
        .ReturnsAsync(User);

    //Act
    var result = await _handler.Handle(new UpdateUserCommand(User), CancellationToken.None);

    //Assert
    Assert.NotNull(result);
    User.Should().BeEquivalentTo(result);
  }
}

using Moq;
using Rira.CrudTest.Core.Interfaces.DataAccess;
using Rira.CrudTest.UnitTests.Initialization;
using Rira.CrudTest.UseCases.Users.Delete;
using Rira.CrudTest.UseCases.Users.Get;
using Xunit;

namespace Rira.CrudTest.UnitTests.UseCases.Users;

public class DeleteUserHandlerHandle
{
  private readonly Mock<IUserDataAccessService> _userDataAccessServiceMock = new();
  private readonly DeleteUserHandler _deleteHandler;

  public DeleteUserHandlerHandle()
  {
    _deleteHandler = new DeleteUserHandler(_userDataAccessServiceMock.Object);
  }

  [Fact]
  public async Task Returns_Success()
  {
    // Arrange
    var init = new UserInitialization();
    init.CreateUsers();
    var User = init.testUserList[0];

    _userDataAccessServiceMock
        .Setup(service => service.DeleteAsync(It.IsAny<Guid>()))
        .ReturnsAsync(true);

    //Act
    var result = await _deleteHandler.Handle(new DeleteUserCommand(User.Id.ToString()), CancellationToken.None);

    //Assert
    Assert.True(result);
  }
}

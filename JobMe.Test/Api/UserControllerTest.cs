using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using JobMe.Api.Controllers;
using Domain.Entities;
using Features.Users.GetAll;
using Features.Users.GetById;
using Features.Users.Add;
using Features.Users.Update;
using Features.Users.Delete;
using Application.Responses;

namespace JobMe.Test.Api
{
    public class UserControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _controller;
        private BaseResponse _response = new BaseResponse() { IsSuccessful = true, Message="Tested" };	

        public UserControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            _response.Data = new List<User>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllUserQueryRequest>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(_response));

            // Act
            var result = await _controller.GetAll(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<BaseResponse>(okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            _response.Data = new User { Id = userId };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetByIdUserQueryRequest>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(_response));

            // Act
            var result = await _controller.GetById(userId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<BaseResponse>(okResult.Value);
            var userData = user.Data as User;
            Assert.NotNull(userData);
            Assert.Equal(userId, userData.Id);
        }

        [Fact]
        public async Task AddUser_ReturnsOkResult()
        {
            // Arrange
            var user = new User { Id = 1, Username = "Test User" };
            _response.Data = user;
            _mediatorMock.Setup(m => m.Send(It.IsAny<AddUserCommandRequest>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(_response));

            // Act
            var result = await _controller.AddUser(user, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var addedUser = Assert.IsType<BaseResponse>(okResult.Value);
            var addedUserData = addedUser.Data as User;
            Assert.NotNull(addedUserData);
            Assert.Equal(user.Id, addedUserData.Id);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "Updated User" };
            _response.Data = user;
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommandRequest>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(_response));

            // Act
            var result = await _controller.UpdateUser(userId, user, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedUser = Assert.IsType<BaseResponse>(okResult.Value);
            var updatedUserData = updatedUser.Data as User;
            Assert.NotNull(updatedUserData);
            Assert.Equal(userId, updatedUserData.Id);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            _response.Data = true;
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommandRequest>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(_response));


            // Act
            var result = await _controller.Delete(userId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<BaseResponse>(okResult.Value);
        }
    }
}


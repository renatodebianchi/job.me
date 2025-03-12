using Api.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace JobMe.Test
{
    public class WebSocketHubTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IHubCallerClients> _clientsMock;
        private readonly Mock<IGroupManager> _groupsMock;
        private readonly Mock<HubCallerContext> _contextMock;
        private readonly WebSocketHub _hub;

        public WebSocketHubTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _clientsMock = new Mock<IHubCallerClients>();
            _groupsMock = new Mock<IGroupManager>();
            _contextMock = new Mock<HubCallerContext>();

            _hub = new WebSocketHub(_mediatorMock.Object)
            {
                Clients = _clientsMock.Object,
                Groups = _groupsMock.Object,
                Context = _contextMock.Object
            };
        }

        [Fact]
        public async Task OnConnectedAsync_ShouldSubscribeToGroupAndSendMessages()
        {
            // Arrange
            var connectionId = "test-connection-id";
            _contextMock.Setup(c => c.ConnectionId).Returns(connectionId);
            var clientProxyMock = new Mock<IClientProxy>();
            _clientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);
            _clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(clientProxyMock.Object);

            // Act
            await _hub.OnConnectedAsync();

            // Assert
            _groupsMock.Verify(g => g.AddToGroupAsync(connectionId, "GeneralGroup", It.IsAny<CancellationToken>()), Times.Once);
            clientProxyMock.Verify(c => c.SendCoreAsync("clientFunctionCallbackName", new object?[] { "TesteSocketApi" }, It.IsAny<CancellationToken>()), Times.Once);
            clientProxyMock.Verify(c => c.SendCoreAsync("clientFunctionCallbackName", new object?[] { "Group Message" }, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OnDisconnectedAsync_ShouldCallBaseMethod()
        {
            // Act
            await _hub.OnDisconnectedAsync(null);

            // Assert
            // No additional assertions needed as we are just verifying the method completes without exceptions
        }

        [Fact]
        public async Task SubscribeAsync_ShouldAddConnectionToGroup()
        {
            // Arrange
            var connectionId = "test-connection-id";
            _contextMock.Setup(c => c.ConnectionId).Returns(connectionId);

            // Act
            await _hub.SubscribeAsync("TestGroup");

            // Assert
            _groupsMock.Verify(g => g.AddToGroupAsync(connectionId, "TestGroup", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldSendMessageToGroup()
        {
            // Arrange
            var clientProxyMock = new Mock<IClientProxy>();
            _clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(clientProxyMock.Object);

            // Act
            await _hub.SendMessageAsync("TestGroup", "TestCallback", "TestMessage");

            // Assert
            clientProxyMock.Verify(c => c.SendCoreAsync("TestCallback", new object?[] { "TestMessage" }, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteSomeTask_ShouldReturnServiceOnline()
        {
            // Act
            var result = await _hub.ExecuteSomeTask("TestGroup", new string[] { }, CancellationToken.None);

            // Assert
            Assert.Equal("Service Online", result);
        }
    }
}
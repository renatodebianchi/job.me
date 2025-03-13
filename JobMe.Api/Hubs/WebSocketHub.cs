using MediatR; 
using Microsoft.AspNetCore.SignalR; 
namespace Api.Hubs 
{ 
    /// <summary>
    /// Represents a SignalR hub for handling WebSocket connections.
    /// </summary>
    public class WebSocketHub : Hub 
    { 
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketHub"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        public WebSocketHub(IMediator mediator) 
        { 
            _mediator = mediator; 
        } 

        /// <summary>
        /// Called when a new connection is established.
        /// </summary>
        public override async Task OnConnectedAsync() 
        { 
            await base.OnConnectedAsync(); 
            // This newMessage call is what is not being received on the front end 
            // This console.WriteLine does print when I bring up the component in the front end. 
            Console.WriteLine("Nova conexao: " + Context.ConnectionId); 
            await SubscribeAsync("GeneralGroup"); 
            await Clients.All.SendAsync("clientFunctionCallbackName", "TesteSocketApi"); 
            await SendMessageAsync("GeneralGroup", "clientFunctionCallbackName", "Group Message"); 
        } 

        /// <summary>
        /// Called when a connection is disconnected.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public override Task OnDisconnectedAsync(Exception? exception) 
        { 
            return base.OnDisconnectedAsync(exception); 
        }

        /// <summary>
        /// Called to subscribe to a group.
        /// </summary>
        /// <param name="group">The group to subscribe.</param>
        public async Task SubscribeAsync(string group) 
        { 
            await Groups.AddToGroupAsync(Context.ConnectionId, group); 
        } 

        /// <summary>
        /// Sends a message to a specified group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="callBackName">The callback name.</param>
        /// <param name="message">The message to send.</param>
        public async Task SendMessageAsync(string group, string callBackName, string message) 
        { 
            //await Clients.All.SendAsync("test", message); 
            await Clients.Group(group).SendAsync(callBackName, message); 
        } 

        /// <summary>
        /// Executes a task and returns a result.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="args">The arguments for the task.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the task.</returns>
        public async Task<string> ExecuteSomeTask(string group, string[] args, CancellationToken cancellationToken) 
        { 
            /*Console.WriteLine("Nova conexao: " + Context.ConnectionId); 
            await SubscribeAsync("GeneralGroup"); 
            await Clients.All.SendAsync("clientFunctionCallbackName", "TesteSocketApi"); 
            await SendMessageAsync("GeneralGroup", "clientFunctionCallbackName", "Group Message"); */

            /*IRequest request = new ListAllPedidoRequest(); 
            var retorno = await _mediator.Send(request, cancellationToken);*/ 
            await Task.Delay(1); 
            var retorno = "Service Online"; 
            return retorno; 
        } 
    } 

}      
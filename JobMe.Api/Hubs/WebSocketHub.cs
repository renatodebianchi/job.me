using MediatR; 
using System; 
using Microsoft.AspNetCore.SignalR; 
using System.Threading; 
using System.Threading.Tasks; 
namespace Api.Hubs 
{ 
    public class WebSocketHub : Hub 
    { 
        private readonly IMediator _mediator; 
        public WebSocketHub(IMediator mediator) 
        { 
            _mediator = mediator; 
        } 
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
        public override Task OnDisconnectedAsync(Exception exception) 
        { 
            return base.OnDisconnectedAsync(exception); 
        } 
        public async Task SubscribeAsync(string group) 
        { 
            await Groups.AddToGroupAsync(Context.ConnectionId, group); 
        } 
        public async Task SendMessageAsync(string group, string callBackName, string message) 
        { 
            //await Clients.All.SendAsync("test", message); 
            await Clients.Group(group).SendAsync(callBackName, message); 
        } 
        public async Task<string> ExecuteSomeTask(string group, string[] args, CancellationToken cancellationToken) 
        { 
            /*IRequest request = new ListAllPedidoRequest(); 
            var retorno = await _mediator.Send(request, cancellationToken);*/ 
            await Task.Delay(1); 
            var retorno = "Service Online"; 
            return retorno; 
        } 
    } 
} 

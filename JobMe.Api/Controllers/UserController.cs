using MediatR; 
using Microsoft.AspNetCore.Mvc; 
using Domain.Entities;
namespace JobMe.Api.Controllers 
{ 
    [ApiController] 
    [Route("[controller]")] 
    public class UserController : ControllerBase 
    { 
        private readonly IMediator _mediator; 
        public UserController(IMediator mediator) 
        { 
            _mediator = mediator; 
        } 
        [HttpGet] 
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.GetAll.GetAllUserQueryRequest(); 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        } 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.GetById.GetByIdUserQueryRequest() { Id = id }; 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        } 

        [HttpPost] 
        public async Task<IActionResult> AddUser(User user, CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.Add.AddUserCommandRequest { DataModel = user }; 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        } 

        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateUser(int id, User user, CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.Update.UpdateUserCommandRequest { Id = id, DataModel = user }; 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        } 

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.Delete.DeleteUserCommandRequest() { Id = id }; 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        }
    } 


} 

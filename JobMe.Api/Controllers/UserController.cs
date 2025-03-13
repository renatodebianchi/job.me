using MediatR; 
using Microsoft.AspNetCore.Mvc; 
using Domain.Entities;
namespace JobMe.Api.Controllers 
{ 
    /// <summary>
    /// Controller for managing user-related actions.
    /// </summary>
    [ApiController] 
    [Route("[controller]")] 
    public class UserController : ControllerBase 
    { 
        private readonly IMediator _mediator; 
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        public UserController(IMediator mediator) 
        { 
            _mediator = mediator; 
        } 
        
        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A list of users.</returns>
        [HttpGet] 
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.GetAll.GetAllUserQueryRequest(); 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        } 

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The user with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.GetById.GetByIdUserQueryRequest() { Id = id }; 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        } 

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost] 
        public async Task<IActionResult> AddUser(User user, CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.Add.AddUserCommandRequest { DataModel = user }; 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        } 

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="user">The updated user data.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateUser(int id, User user, CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.Update.UpdateUserCommandRequest { Id = id, DataModel = user }; 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        } 

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) 
        { 
            var request = new Features.Users.Delete.DeleteUserCommandRequest() { Id = id }; 
            var retorno = await _mediator.Send(request, cancellationToken);
            
            return Ok(retorno); 
        }
    } 


} 

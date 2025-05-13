using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using Domain.Entities;

namespace JobMe.Api.Controllers
{
    /// <summary>
    /// Controller for managing character-related operations.
    /// </summary>
    [ApiController] 
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly IGenericRepository<Character> _characterRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterController"/> class.
        /// </summary>
        /// <param name="characterRepository">The repository for managing character data.</param>
        public CharacterController(IGenericRepository<Character> characterRepository)
        {
            _characterRepository = characterRepository;
        }

        
        /// <summary>
        /// Uploads an avatar for a specific character.
        /// </summary>
        /// <param name="avatar">The avatar file to upload.</param>
        /// <param name="characterId">The ID of the character to associate the avatar with.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar, [FromForm] int characterId)
        {
            if (avatar == null || avatar.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var character = await _characterRepository.GetByIdAsync(characterId);
            if (character == null)
            {
                return NotFound("Character not found.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, avatar.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatar.CopyToAsync(stream);
            }

            // Update character with avatar path (if needed)
            character.AvatarPath = $"/uploads/{avatar.FileName}";
            await _characterRepository.UpdateAsync(character);

            return Ok(new { Message = "Avatar uploaded successfully.", AvatarPath = character.AvatarPath });
        }
    }
}

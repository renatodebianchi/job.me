using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : GenericEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
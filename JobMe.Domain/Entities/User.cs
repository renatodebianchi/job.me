using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : GenericEntity
    {
        public User() : base()
        {
            Username = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
        }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
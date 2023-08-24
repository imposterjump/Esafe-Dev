using System.ComponentModel.DataAnnotations;

namespace BankProject.Models.Client.Request
{
    public class ClientLoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}

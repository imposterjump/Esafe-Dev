using System.ComponentModel.DataAnnotations;

namespace BankProject.Models
{
    public class ClientDisplayDto
    {

        [Key]
        [MaxLength(14, ErrorMessage = "National ID must be 14 numbers.")]
        [Required(ErrorMessage = "National ID is required.")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address is invalid.")]
        public string Email { get; set; }
    }
}

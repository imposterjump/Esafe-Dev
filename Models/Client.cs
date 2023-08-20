using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankProject.Models
{
    public class Client : BaseEntity
    {
        public Client()
        {
            Addresses = new HashSet<Address>();
        }

        [Key]
        [MaxLength(14, ErrorMessage = "National ID must be 14 numbers.")]
        [Required(ErrorMessage = "National ID is required.")]
        public string NationalId { get; set; }

        public ICollection<Address> Addresses { get; set; }


        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address is invalid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password should be atleast 8 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Birthdate is required.")]
        public DateTime Birthdate { get; set; }


    }
}

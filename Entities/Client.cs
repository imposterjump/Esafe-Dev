using BankProject.Data.Enums;
using BankProject.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace BankProject.Entities
{
    public class Client:User
    {
        
      

        [MaxLength(14, ErrorMessage = "National ID must be 14 numbers.")]
        [Required(ErrorMessage = "National ID is required.")]
        public string NationalId { get; set; }

        [Phone(ErrorMessage = "Phone Number is invalid.")]
        public string PhoneNumber { get; set; }
        public string? AccountNo { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Blocked { get; set; } = false;
        public List<Address>? ClientAddresses { get; set; } = new List<Address>();
    }
}
using BankProject.Models.Address;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BankProject.Models.Client.Response
{
    public class ClientDisplayDto
    {
        public string NationalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AccountNo { get; set; }
        public List<AddressDto>? ClientAddresses { get; set; } = new List<AddressDto>();
    }
}

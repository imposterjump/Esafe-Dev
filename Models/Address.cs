using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BankProject.Models
{
    public class Address
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClientID { get; set; }

        [IgnoreDataMember]
        public Client? Client { get; set; }


        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        public string? Street { get; set; }

        public Address(string street, string country, string city)
        {
            if(street != null)
            {
                this.Street = street;
            }
            this.Country = country;
            this.City = city;
        }


    }
}

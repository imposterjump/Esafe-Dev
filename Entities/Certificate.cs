using esafe_final_inshallah.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace esafe_final_inshallah.Models
{
    public class Certificate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public CertificateType CertificateType { get; set; }
        public bool Accepted { get; set; } = false;
        public DateTime ApplicationDate { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public int? ApprovedById { get; set; }
    }
}

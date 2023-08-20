using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankProject.Models
{
    public class News : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 letters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Descroption is required")]
        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 letters.")]
        public string Description { get; set; }
    }
}

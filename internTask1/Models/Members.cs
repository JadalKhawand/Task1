using System.ComponentModel.DataAnnotations;

namespace internTask1.Models
{
    public class Members
    {
        [Key]
        public int MemberID { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Email { get; set; }
    }
}

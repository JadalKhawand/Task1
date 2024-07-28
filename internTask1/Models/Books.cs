using System.ComponentModel.DataAnnotations;

namespace internTask1.Models
{
    public class Books
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Author { get; set; }

        [Required]
        public required string ISBN { get; set; }
    }
}

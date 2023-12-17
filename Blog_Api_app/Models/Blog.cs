using System.ComponentModel.DataAnnotations;

namespace Blog_Api_app.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Aurthor { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Content { get; set; }


    }
}

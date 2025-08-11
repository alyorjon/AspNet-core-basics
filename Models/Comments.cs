using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Comment")]
    public class Comment
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        
        // Foreign Keys
        public int? StockId { get; set; }
        public string? AppUserId { get; set; }
        
        // Navigation Properties
        public Stock? Stock { get; set; }
        public AppUser? AppUser { get; set; } // Bu property kerak edi
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public Guid id { get; set; }
        // Foreign Keys
        public string AppUserId { get; set; } = string.Empty;
        public int StockId { get; set; }
        
        // Navigation Properties
        public AppUser AppUser { get; set; } = null!;
        public Stock Stock { get; set; } = null!;
    }
}
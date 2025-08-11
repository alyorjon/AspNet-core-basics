using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Stocks")]
    public class Stock
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        
        public long MarketCap { get; set; }
        
        [MaxLength(500)]
        public string Industry { get; set; } = string.Empty;
        
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        
        // Navigation Properties
        public List<Comment> Comment { get; set; } = new List<Comment>();
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
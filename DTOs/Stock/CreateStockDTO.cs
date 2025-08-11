

using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Stock;

public class CreateStockDTO
{
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol 10 ta belgidan oshmasligi kerak")]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [MaxLength(100, ErrorMessage = "CompanyName 100 ta belgidan oshmasligi kerak")]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 1000000, ErrorMessage = "Price 0.01 va 1000000 orasida bo'lishi kerak")]
    public decimal Price { get; set; }
    [Required]
    public long MarketCap { get; set; }
    
    [Required]
    public string Industry { get; set; } = string.Empty;
}


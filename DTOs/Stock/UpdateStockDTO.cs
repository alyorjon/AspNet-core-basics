using System;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Stock;

public class UpdateStockDTO
{
    [MaxLength(10)]
    public string? Symbol { get; set; }

    [MaxLength(100)]
    public string? CompanyName { get; set; }

    [Range(0.01, 1000000)]
    public decimal? Price { get; set; }
    public long? MarketCap { get; set; }
    public string? Industry { get; set; }
}
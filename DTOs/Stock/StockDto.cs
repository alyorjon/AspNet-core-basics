using System;
using api.DTOs.Comment;

namespace api.DTOs;

public class StockDto
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime LastUpdated { get; set; }
    public long MarketCap { get; set; }
    public string Industry { get; set; } = string.Empty;
}

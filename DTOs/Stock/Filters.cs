using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Stock;

    // StockFilterDto.cs
public class StockFilterDto
{
    public string? Symbol { get; set; }
    public string? CompanyName { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public DateTime? UpdatedAfter { get; set; }
    public DateTime? UpdatedBefore { get; set; }
    public bool? HasComment { get; set; }
}

// StockStatisticsDto.cs
public class StockStatisticsDto
{
    public int TotalStocks { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal TotalMarketValue { get; set; }
    public int StocksWithComment { get; set; }
    public int StocksWithoutComment { get; set; }
}

// StocksByPriceRangeDto.cs
public class StocksByPriceRangeDto
{
    public string PriceRange { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal AveragePrice { get; set; }
}

// StockGroupDto.cs
public class StockGroupDto
{
    public string GroupName { get; set; } = string.Empty;
    public int Count { get; set; }
    public List<StockDto> Stocks { get; set; } = new();
}

// StockWithCommentDto.cs
public class StockWithCommentDto
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CommentCount { get; set; }
    public DateTime LastUpdated { get; set; }
}

// PagedResultDto.cs
public class PagedResultDto<T>
{
    public List<T> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
public class ValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }
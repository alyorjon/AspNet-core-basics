using System;
using api.DTOs;
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stockModel)
    {
        return new StockDto
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
            Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
        };
    }
    public static Stock ToStockFromCreateDTO(this CreateStockDTO createDto)
    {
        return new Stock
        {
            Symbol = createDto.Symbol,
            CompanyName = createDto.CompanyName,
            Purchase = createDto.Purchase,
            LastDiv = createDto.LastDiv,
            Industry = createDto.Industry,
            MarketCap = createDto.MarketCap
        };
    }
}

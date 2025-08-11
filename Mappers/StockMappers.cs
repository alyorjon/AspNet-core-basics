using api.DTOs;
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Price = stockModel.Price,
                LastUpdated = stockModel.LastUpdated,
                MarketCap = stockModel.MarketCap,
                Industry = stockModel.Industry,
                // Comment = stockModel.Comment
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockDTO stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Price = stockDto.Price,
                LastUpdated = DateTime.Now
            };
        }
    }
}
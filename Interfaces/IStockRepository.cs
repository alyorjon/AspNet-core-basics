using System;
using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();
    Task<Stock> GetStockByIdAsync(int id);
    Task<Stock> CreateStockAsync(Stock stockModel);
    Task<Stock> UpdateStockAsync(int id, UpdateStockDTO stockDto);
    Task<Stock> DeleteStockAsync(int id);
}

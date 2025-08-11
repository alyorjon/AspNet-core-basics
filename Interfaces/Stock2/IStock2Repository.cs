using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces.Stock2
{
    public interface IStock2Repository
    {
        Task<Stock> GetAll();
        Task<Stock> GetById(int id);
        Task<Stock> CreateStock(Stock stock);
        Task<Stock> UpdateStock(int id, UpdateStockDTO updateStockDTO);
        Task<Stock> DeleteStock(int id);
    }
}
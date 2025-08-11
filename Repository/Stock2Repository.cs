using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.ApplicationDbContext;
using api.DTOs.Stock;
using api.Interfaces.Stock2;
using api.Models;

namespace api.Repository
{
    public class Stock2Repository : IStock2Repository
    {

        private readonly ApplicationDBContext _context;
        public Stock2Repository(ApplicationDBContext context)
        {
            _context = context;
        }
        public Task<Stock> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Stock> GetById(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<Stock> CreateStock(Stock stock)
        {
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();
            return stock;
        }


        public Task<Stock> UpdateStock(int id, UpdateStockDTO updateStockDTO)
        {
            throw new NotImplementedException();
        }

        public Task<Stock> DeleteStock(int id)
        {
            throw new NotImplementedException();
        }
    }
}
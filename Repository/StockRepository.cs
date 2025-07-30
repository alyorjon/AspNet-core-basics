using System;
using api.ApplicationDbContext;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context;
    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stock.Include(c=>c.Comments).ToListAsync();
    }
    public async Task<Stock> GetStockByIdAsync(int id)
    {
        return await _context.Stock.Include(c=>c.Comments).FirstOrDefaultAsync(i=>i.Id== id);
    }

    public async Task<Stock> CreateStockAsync(Stock stockModel)
    {
        var stock = new Stock
        {
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap
        };
        await _context.Stock.AddAsync(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock> UpdateStockAsync(int id, UpdateStockDTO stockDto)
    {
        var stock = await _context.Stock.Include(c=>c.Comments).FirstOrDefaultAsync(x => x.Id == id);
        if (stock == null) return null;

        stock.Symbol = stockDto.Symbol;
        stock.CompanyName = stockDto.CompanyName;
        stock.Purchase = stockDto.Purchase;
        stock.LastDiv = stockDto.LastDiv;
        stock.Industry = stockDto.Industry;
        stock.MarketCap = stockDto.MarketCap;

        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock> DeleteStockAsync(int id)
    {
        var stock = await _context.Stock.FindAsync( id);
        if (stock == null) return null;

        _context.Stock.Remove(stock);
        await _context.SaveChangesAsync();
        return stock;
    }
}

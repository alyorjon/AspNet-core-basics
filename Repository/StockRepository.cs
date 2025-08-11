using api.ApplicationDbContext;
// using api.Data;
using api.DTOs;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

/*************  ✨ Windsurf Command ⭐  *************/
/// <summary>
/// Asynchronously retrieves all stocks from the database.
/// </summary>
/// <returns>A task that represents the asynchronous operation. The task result contains a list of all stocks.</returns>

/*******  40cda6ca-6c03-4e2f-b487-42b8f298a9dd  *******/        // Basic CRUD (oldingi kodlar)
        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks
                .OrderBy(x => x.Symbol)
                .ToListAsync();
        }

        public async Task<Stock> GetByIdAsync(int id)
        {
            var res = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            return res;
        }
        // Complex Queries
        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks
                .FirstOrDefaultAsync(s => s.Symbol.ToLower() == symbol.ToLower());
        }

        public async Task<List<Stock>> GetByCompanyNameAsync(string companyName)
        {
            return await _context.Stocks
                .Where(s => s.CompanyName.Contains(companyName))
                .OrderBy(s => s.CompanyName)
                .ToListAsync();
        }

        public async Task<List<Stock>> GetStocksWithCommentAsync()
        {
            return await _context.Stocks
                .Include(s => s.Comment) // Related data yuklanadi
                .Where(s => s.Comment.Any())
                .ToListAsync();
        }

        public async Task<Stock?> GetStockWithCommentAsync(int id)
        {
            return await _context.Stocks
                .Include(s => s.Comment)
                .ThenInclude(c => c.AppUser) // Nested include
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Stock>> GetStocksByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Stocks
                .Where(s => s.Price >= minPrice && s.Price <= maxPrice)
                .OrderBy(s => s.Price)
                .ToListAsync();
        }

        public async Task<List<Stock>> GetTopExpensiveStocksAsync(int count)
        {
            return await _context.Stocks
                .OrderByDescending(s => s.Price)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Stock>> GetRecentlyUpdatedStocksAsync(int days)
        {
            var cutoffDate = DateTime.Now.AddDays(-days);
            
            return await _context.Stocks
                .Where(s => s.LastUpdated >= cutoffDate)
                .OrderByDescending(s => s.LastUpdated)
                .ToListAsync();
        }

        // Filtering & Sorting
        public async Task<List<Stock>> GetFilteredStocksAsync(StockFilterDto filter)
        {
            var query = _context.Stocks.AsQueryable();

            // Dynamic filtering
            if (!string.IsNullOrWhiteSpace(filter.Symbol))
            {
                query = query.Where(s => s.Symbol.Contains(filter.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(filter.CompanyName))
            {
                query = query.Where(s => s.CompanyName.Contains(filter.CompanyName));
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(s => s.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(s => s.Price <= filter.MaxPrice.Value);
            }

            if (filter.UpdatedAfter.HasValue)
            {
                query = query.Where(s => s.LastUpdated >= filter.UpdatedAfter.Value);
            }

            if (filter.UpdatedBefore.HasValue)
            {
                query = query.Where(s => s.LastUpdated <= filter.UpdatedBefore.Value);
            }

            if (filter.HasComment.HasValue)
            {
                if (filter.HasComment.Value)
                {
                    query = query.Where(s => s.Comment.Any());
                }
                else
                {
                    query = query.Where(s => !s.Comment.Any());
                }
            }

            return await query.OrderBy(s => s.Symbol).ToListAsync();
        }

        public async Task<List<Stock>> GetSortedStocksAsync(string sortBy, bool descending = false)
        {
            var query = _context.Stocks.AsQueryable();

            // Dynamic sorting
            query = sortBy.ToLower() switch
            {
                "symbol" => descending ? query.OrderByDescending(s => s.Symbol) 
                                      : query.OrderBy(s => s.Symbol),
                "companyname" => descending ? query.OrderByDescending(s => s.CompanyName) 
                                           : query.OrderBy(s => s.CompanyName),
                "price" => descending ? query.OrderByDescending(s => s.Price) 
                                     : query.OrderBy(s => s.Price),
                "lastupdated" => descending ? query.OrderByDescending(s => s.LastUpdated) 
                                           : query.OrderBy(s => s.LastUpdated),
                _ => query.OrderBy(s => s.Id)
            };

            return await query.ToListAsync();
        }

        public async Task<(List<Stock>, int)> GetPagedStocksAsync(int pageNumber, int pageSize, string sortBy = "Id")
        {
            var query = _context.Stocks.AsQueryable();

            // Sorting
            query = sortBy.ToLower() switch
            {
                "symbol" => query.OrderBy(s => s.Symbol),
                "companyname" => query.OrderBy(s => s.CompanyName),
                "price" => query.OrderBy(s => s.Price),
                "lastupdated" => query.OrderByDescending(s => s.LastUpdated),
                _ => query.OrderBy(s => s.Id)
            };

            var totalCount = await query.CountAsync();
            
            var stocks = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (stocks, totalCount);
        }

        // Search
        public async Task<List<Stock>> SearchStocksAsync(string searchTerm)
        {
            return await _context.Stocks
                .Where(s => s.Symbol.Contains(searchTerm) || 
                           s.CompanyName.Contains(searchTerm))
                .OrderBy(s => s.Symbol)
                .ToListAsync();
        }

        public async Task<List<Stock>> SearchStocksBySymbolAsync(string symbolPattern)
        {
            return await _context.Stocks
                .Where(s => EF.Functions.Like(s.Symbol, symbolPattern)) // SQL LIKE operator
                .OrderBy(s => s.Symbol)
                .ToListAsync();
        }

        // Aggregations
        public async Task<int> GetTotalStockCountAsync()
        {
            return await _context.Stocks.CountAsync();
        }

        public async Task<decimal> GetAverageStockPriceAsync()
        {
            return await _context.Stocks.AverageAsync(s => s.Price);
        }

        public async Task<decimal> GetTotalMarketValueAsync()
        {
            return await _context.Stocks.SumAsync(s => s.Price);
        }

        public async Task<StockStatisticsDto> GetStockStatisticsAsync()
        {
            // Bir marta query bilan barcha statistika
            var stats = await _context.Stocks
                .GroupBy(s => 1) // Barcha recordlarni bitta groupga sol
                .Select(g => new StockStatisticsDto
                {
                    TotalStocks = g.Count(),
                    AveragePrice = g.Average(s => s.Price),
                    MinPrice = g.Min(s => s.Price),
                    MaxPrice = g.Max(s => s.Price),
                    TotalMarketValue = g.Sum(s => s.Price),
                    StocksWithComment = g.Count(s => s.Comment.Any()),
                    StocksWithoutComment = g.Count(s => !s.Comment.Any())
                })
                .FirstOrDefaultAsync();

            return stats ?? new StockStatisticsDto();
        }

        public async Task<List<StocksByPriceRangeDto>> GetStocksByPriceRangesAsync()
        {
            return await _context.Stocks
                .GroupBy(s => s.Price < 100 ? "0-100" :
                             s.Price < 500 ? "100-500" :
                             s.Price < 1000 ? "500-1000" : "1000+")
                .Select(g => new StocksByPriceRangeDto
                {
                    PriceRange = g.Key,
                    Count = g.Count(),
                    AveragePrice = g.Average(s => s.Price)
                })
                .OrderBy(x => x.PriceRange)
                .ToListAsync();
        }

        // Date-based queries
        public async Task<List<Stock>> GetStocksUpdatedInRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Stocks
                .Where(s => s.LastUpdated >= startDate && s.LastUpdated <= endDate)
                .OrderByDescending(s => s.LastUpdated)
                .ToListAsync();
        }

        public async Task<List<Stock>> GetStocksUpdatedTodayAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            
            return await _context.Stocks
                .Where(s => s.LastUpdated >= today && s.LastUpdated < tomorrow)
                .OrderByDescending(s => s.LastUpdated)
                .ToListAsync();
        }

        public async Task<List<Stock>> GetStocksUpdatedThisWeekAsync()
        {
            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            
            return await _context.Stocks
                .Where(s => s.LastUpdated >= startOfWeek)
                .OrderByDescending(s => s.LastUpdated)
                .ToListAsync();
        }

        public async Task<List<Stock>> GetStocksUpdatedThisMonthAsync()
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            
            return await _context.Stocks
                .Where(s => s.LastUpdated >= startOfMonth)
                .OrderByDescending(s => s.LastUpdated)
                .ToListAsync();
        }

        // Grouping
        public async Task<List<StockGroupDto>> GetStocksGroupedByPriceRangeAsync()
        {
            return await _context.Stocks
                .GroupBy(s => s.Price < 100 ? "Arzon (0-100)" :
                             s.Price < 500 ? "O'rtacha (100-500)" :
                             s.Price < 1000 ? "Qimmat (500-1000)" : "Juda qimmat (1000+)")
                .Select(g => new StockGroupDto
                {
                    GroupName = g.Key,
                    Count = g.Count(),
                    Stocks = g.Select(s => new StockDto
                    {
                        Id = s.Id,
                        Symbol = s.Symbol,
                        CompanyName = s.CompanyName,
                        Price = s.Price,
                        LastUpdated = s.LastUpdated
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetStockCountByFirstLetterAsync()
        {
            return await _context.Stocks
                .GroupBy(s => s.Symbol.Substring(0, 1).ToUpper())
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        // Joins & Related Data
        public async Task<List<StockWithCommentDto>> GetStocksWithCommentCountAsync()
        {
            return await _context.Stocks
                .Select(s => new StockWithCommentDto
                {
                    Id = s.Id,
                    Symbol = s.Symbol,
                    CompanyName = s.CompanyName,
                    Price = s.Price,
                    CommentCount = s.Comment.Count(),
                    LastUpdated = s.LastUpdated
                })
                .ToListAsync();
        }

        public async Task<List<Stock>> GetStocksWithMostCommentAsync(int count)
        {
            return await _context.Stocks
                .Include(s => s.Comment)
                .OrderByDescending(s => s.Comment.Count())
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Stock>> GetStocksWithoutCommentAsync()
        {
            return await _context.Stocks
                .Where(s => !s.Comment.Any())
                .OrderBy(s => s.Symbol)
                .ToListAsync();
        }

        // Advanced operations
        public async Task<bool> BulkUpdatePricesAsync(Dictionary<int, decimal> priceUpdates)
        {
            var stockIds = priceUpdates.Keys.ToList();
            var stocks = await _context.Stocks
                .Where(s => stockIds.Contains(s.Id))
                .ToListAsync();

            foreach (var stock in stocks)
            {
                if (priceUpdates.TryGetValue(stock.Id, out var newPrice))
                {
                    stock.Price = newPrice;
                    stock.LastUpdated = DateTime.Now;
                }
            }

            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<List<Stock>> ExecuteRawSqlAsync(string sql, params object[] parameters)
        {
            return await _context.Stocks
                .FromSqlRaw(sql, parameters)
                .ToListAsync();
        }

        public async Task<int> GetStockCountBySqlAsync(string whereClause)
        {
            var sql = $"SELECT COUNT(*) FROM Stocks WHERE {whereClause}";
            
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            
            await _context.Database.OpenConnectionAsync();
            var result = await command.ExecuteScalarAsync();
            
            return Convert.ToInt32(result);
        }

        // Performance queries
        public async Task<List<Stock>> GetStocksAsNoTrackingAsync()
        {
            return await _context.Stocks
                .AsNoTracking() // Change tracking yo'q, tezroq
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Stocks
                .AnyAsync(s => s.Id == id); // Exists check - tez
        }

        public async Task<bool> ExistsBySymbolAsync(string symbol)
        {
            return await _context.Stocks
                .AnyAsync(s => s.Symbol == symbol);
        }

        // Original CRUD methods
        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock> UpdateAsync(int id, UpdateStockDTO stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            
            if (existingStock == null)
                return null;

            if (!string.IsNullOrWhiteSpace(stockDto.Symbol))
                existingStock.Symbol = stockDto.Symbol;
                
            if (!string.IsNullOrWhiteSpace(stockDto.CompanyName))
                existingStock.CompanyName = stockDto.CompanyName;
                
            if (stockDto.Price.HasValue)
                existingStock.Price = stockDto.Price.Value;
            if (!string.IsNullOrWhiteSpace(stockDto.Industry))
                existingStock.Industry = stockDto.Industry;
            if (stockDto.MarketCap.HasValue)
                existingStock.MarketCap = stockDto.MarketCap.Value; 
            existingStock.LastUpdated = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingStock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            
            if (stockModel == null)
                return null;

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }
    }
}
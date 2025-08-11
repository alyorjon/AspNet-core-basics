using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        // Basic CRUD
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockDTO stockDto);
        Task<Stock?> DeleteAsync(int id);

        // Complex Queries
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<List<Stock>> GetByCompanyNameAsync(string companyName);
        Task<List<Stock>> GetStocksWithCommentAsync();
        Task<Stock?> GetStockWithCommentAsync(int id);
        Task<List<Stock>> GetStocksByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<List<Stock>> GetTopExpensiveStocksAsync(int count);
        Task<List<Stock>> GetRecentlyUpdatedStocksAsync(int days);

        // Filtering & Sorting
        Task<List<Stock>> GetFilteredStocksAsync(StockFilterDto filter);
        Task<List<Stock>> GetSortedStocksAsync(string sortBy, bool descending = false);
        Task<(List<Stock>, int)> GetPagedStocksAsync(int pageNumber, int pageSize, string sortBy = "Id");

        // Search
        Task<List<Stock>> SearchStocksAsync(string searchTerm);
        Task<List<Stock>> SearchStocksBySymbolAsync(string symbolPattern);

        // Aggregations
        Task<int> GetTotalStockCountAsync();
        Task<decimal> GetAverageStockPriceAsync();
        Task<decimal> GetTotalMarketValueAsync();
        Task<StockStatisticsDto> GetStockStatisticsAsync();
        Task<List<StocksByPriceRangeDto>> GetStocksByPriceRangesAsync();

        // Date-based queries
        Task<List<Stock>> GetStocksUpdatedInRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Stock>> GetStocksUpdatedTodayAsync();
        Task<List<Stock>> GetStocksUpdatedThisWeekAsync();
        Task<List<Stock>> GetStocksUpdatedThisMonthAsync();

        // Grouping
        Task<List<StockGroupDto>> GetStocksGroupedByPriceRangeAsync();
        Task<Dictionary<string, int>> GetStockCountByFirstLetterAsync();

        // Joins & Related Data
        Task<List<StockWithCommentDto>> GetStocksWithCommentCountAsync();
        Task<List<Stock>> GetStocksWithMostCommentAsync(int count);
        Task<List<Stock>> GetStocksWithoutCommentAsync();

        // Advanced operations
        Task<bool> BulkUpdatePricesAsync(Dictionary<int, decimal> priceUpdates);
        Task<List<Stock>> ExecuteRawSqlAsync(string sql, params object[] parameters);
        Task<int> GetStockCountBySqlAsync(string whereClause);

        // Performance queries
        Task<List<Stock>> GetStocksAsNoTrackingAsync();
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsBySymbolAsync(string symbol);
    }
}
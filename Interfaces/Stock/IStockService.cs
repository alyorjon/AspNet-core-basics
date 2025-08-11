using api.DTOs;
using api.DTOs.Stock;

namespace api.Interfaces
{
    public interface IStockService
    {
        // Basic CRUD Operations
        Task<List<StockDto>> GetAllStocksAsync();
        Task<StockDto?> GetStockByIdAsync(int id);
        Task<StockDto> CreateStockAsync(CreateStockDTO createStockDto);
        Task<StockDto> UpdateStockAsync(int id, UpdateStockDTO updateStockDto);
        Task<bool> DeleteStockAsync(int id);

        // Complex Queries
        Task<StockDto?> GetStockBySymbolAsync(string symbol);
        Task<List<StockDto>> GetStocksByCompanyNameAsync(string companyName);
        Task<List<StockDto>> GetStocksWithCommentAsync();
        Task<StockDto?> GetStockWithCommentAsync(int id);
        Task<List<StockDto>> GetStocksByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<List<StockDto>> GetTopExpensiveStocksAsync(int count);
        Task<List<StockDto>> GetRecentlyUpdatedStocksAsync(int days);

        // Filtering & Sorting
        Task<List<StockDto>> GetFilteredStocksAsync(StockFilterDto filter);
        Task<List<StockDto>> GetSortedStocksAsync(string sortBy, bool descending = false);
        Task<PagedResultDto<StockDto>> GetPagedStocksAsync(int pageNumber, int pageSize, string sortBy = "Id");

        // Search Operations
        Task<List<StockDto>> SearchStocksAsync(string searchTerm);
        Task<List<StockDto>> SearchStocksBySymbolAsync(string symbolPattern);

        // Aggregation Operations
        Task<int> GetTotalStockCountAsync();
        Task<decimal> GetAverageStockPriceAsync();
        Task<decimal> GetTotalMarketValueAsync();
        Task<StockStatisticsDto> GetStockStatisticsAsync();
        Task<List<StocksByPriceRangeDto>> GetStocksByPriceRangesAsync();

        // Date-based Queries
        Task<List<StockDto>> GetStocksUpdatedInRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<StockDto>> GetStocksUpdatedTodayAsync();
        Task<List<StockDto>> GetStocksUpdatedThisWeekAsync();
        Task<List<StockDto>> GetStocksUpdatedThisMonthAsync();

        // Grouping Operations
        Task<List<StockGroupDto>> GetStocksGroupedByPriceRangeAsync();
        Task<Dictionary<string, int>> GetStockCountByFirstLetterAsync();

        // Related Data Operations
        Task<List<StockWithCommentDto>> GetStocksWithCommentCountAsync();
        Task<List<StockDto>> GetStocksWithMostCommentAsync(int count);
        Task<List<StockDto>> GetStocksWithoutCommentAsync();

        // Advanced Operations
        Task<bool> BulkUpdatePricesAsync(Dictionary<int, decimal> priceUpdates);
        Task<List<StockDto>> ExecuteRawSqlAsync(string sql, params object[] parameters);
        Task<int> GetStockCountBySqlAsync(string whereClause);

        // Performance & Utility
        Task<List<StockDto>> GetStocksAsNoTrackingAsync();
        Task<bool> StockExistsAsync(int id);
        Task<bool> StockExistsBySymbolAsync(string symbol);
    }
}
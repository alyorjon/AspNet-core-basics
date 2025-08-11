using Microsoft.AspNetCore.Mvc;
using api.DTOs.Stock;
using api.Interfaces;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<StockController> _logger;

        public StockController(IStockService stockService, ILogger<StockController> logger)
        {
            _stockService = stockService;
            _logger = logger;
        }

        #region Basic CRUD Operations

        /// <summary>
        /// Barcha stocklarni olish
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            try
            {
                var stocks = await _stockService.GetAllStocksAsync();
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllStocks da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// ID bo'yicha stock olish
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockById([FromRoute] int id)
        {
            try
            {
                var stock = await _stockService.GetStockByIdAsync(id);
                if (stock == null)
                    return NotFound($"ID {id} bo'yicha stock topilmadi");

                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStockById da xato: {Id}", id);
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Yangi stock yaratish
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO stockDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdStock = await _stockService.CreateStockAsync(stockDto);
                return CreatedAtAction(nameof(GetStockById), 
                                     new { id = createdStock.Id }, 
                                     createdStock);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateStock da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Stockni yangilash
        /// </summary>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDTO updateStockDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedStock = await _stockService.UpdateStockAsync(id, updateStockDto);
                if (updatedStock == null)
                    return NotFound($"ID {id} bo'yicha stock topilmadi");

                return Ok(updatedStock);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateStock da xato: {Id}", id);
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Stockni o'chirish
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            try
            {
                var isDeleted = await _stockService.DeleteStockAsync(id);
                if (!isDeleted)
                    return NotFound($"ID {id} bo'yicha stock topilmadi");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteStock da xato: {Id}", id);
                return StatusCode(500, "Server xatosi");
            }
        }

        #endregion

        #region Complex Queries
        
        /// <summary>
        /// Symbol bo'yicha stock olish
        /// </summary>
        [HttpGet("symbol/{symbol}")]
        public async Task<IActionResult> GetStockBySymbol([FromRoute] string symbol)
        {
            try
            {
                var stock = await _stockService.GetStockBySymbolAsync(symbol);
                if (stock == null)
                    return NotFound($"Symbol '{symbol}' bo'yicha stock topilmadi");

                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStockBySymbol da xato: {Symbol}", symbol);
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Company name bo'yicha stocklar
        /// </summary>
        [HttpGet("company/{companyName}")]
        public async Task<IActionResult> GetStocksByCompanyName([FromRoute] string companyName)
        {
            try
            {
                var stocks = await _stockService.GetStocksByCompanyNameAsync(companyName);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksByCompanyName da xato: {CompanyName}", companyName);
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Comment mavjud stocklar
        /// </summary>
        [HttpGet("with-Comment")]
        public async Task<IActionResult> GetStocksWithComment()
        {
            try
            {
                var stocks = await _stockService.GetStocksWithCommentAsync();
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksWithComment da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Stock commentlari bilan olish
        /// </summary>
        [HttpGet("{id:int}/with-Comment")]
        public async Task<IActionResult> GetStockWithComment([FromRoute] int id)
        {
            try
            {
                var stock = await _stockService.GetStockWithCommentAsync(id);
                if (stock == null)
                    return NotFound($"ID {id} bo'yicha stock topilmadi");

                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStockWithComment da xato: {Id}", id);
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Price range bo'yicha stocklar
        /// </summary>
        [HttpGet("price-range")]
        public async Task<IActionResult> GetStocksByPriceRange(
            [FromQuery] decimal minPrice, 
            [FromQuery] decimal maxPrice)
        {
            try
            {
                var stocks = await _stockService.GetStocksByPriceRangeAsync(minPrice, maxPrice);
                return Ok(stocks);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksByPriceRange da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Eng qimmat stocklar
        /// </summary>
        [HttpGet("top-expensive")]
        public async Task<IActionResult> GetTopExpensiveStocks([FromQuery] int count = 10)
        {
            try
            {
                var stocks = await _stockService.GetTopExpensiveStocksAsync(count);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetTopExpensiveStocks da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Yaqinda yangilangan stocklar
        /// </summary>
        [HttpGet("recently-updated")]
        public async Task<IActionResult> GetRecentlyUpdatedStocks([FromQuery] int days = 7)
        {
            try
            {
                var stocks = await _stockService.GetRecentlyUpdatedStocksAsync(days);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetRecentlyUpdatedStocks da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        #endregion

        #region Filtering & Sorting

        /// <summary>
        /// Filtr bo'yicha stocklar
        /// </summary>
        [HttpPost("filter")]
        public async Task<IActionResult> GetFilteredStocks([FromBody] StockFilterDto filter)
        {
            try
            {
                var stocks = await _stockService.GetFilteredStocksAsync(filter);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFilteredStocks da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Tartibga solingan stocklar
        /// </summary>
        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedStocks(
            [FromQuery] string sortBy = "Id", 
            [FromQuery] bool descending = false)
        {
            try
            {
                var stocks = await _stockService.GetSortedStocksAsync(sortBy, descending);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetSortedStocks da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Sahifalangan stocklar
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedStocks(
            [FromQuery] int page = 1, 
            [FromQuery] int size = 10, 
            [FromQuery] string sortBy = "Id")
        {
            try
            {
                var result = await _stockService.GetPagedStocksAsync(page, size, sortBy);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPagedStocks da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        #endregion

        #region Search Operations

        /// <summary>
        /// Stocklar qidirish
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchStocks([FromQuery] string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                    return BadRequest("Search term bo'sh bo'lishi mumkin emas");

                var stocks = await _stockService.SearchStocksAsync(term);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SearchStocks da xato: {Term}", term);
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Symbol pattern bo'yicha qidirish
        /// </summary>
        [HttpGet("search-symbol")]
        public async Task<IActionResult> SearchStocksBySymbol([FromQuery] string pattern)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pattern))
                    return BadRequest("Pattern bo'sh bo'lishi mumkin emas");

                var stocks = await _stockService.SearchStocksBySymbolAsync(pattern);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SearchStocksBySymbol da xato: {Pattern}", pattern);
                return StatusCode(500, "Server xatosi");
            }
        }

        #endregion

        #region Aggregation Operations

        /// <summary>
        /// Jami stock soni
        /// </summary>
        [HttpGet("count")]
        public async Task<IActionResult> GetTotalStockCount()
        {
            try
            {
                var count = await _stockService.GetTotalStockCountAsync();
                return Ok(new { TotalCount = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetTotalStockCount da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// O'rtacha stock narxi
        /// </summary>
        [HttpGet("average-price")]
        public async Task<IActionResult> GetAverageStockPrice()
        {
            try
            {
                var avgPrice = await _stockService.GetAverageStockPriceAsync();
                return Ok(new { AveragePrice = avgPrice });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAverageStockPrice da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Jami bozor qiymati
        /// </summary>
        [HttpGet("total-market-value")]
        public async Task<IActionResult> GetTotalMarketValue()
        {
            try
            {
                var totalValue = await _stockService.GetTotalMarketValueAsync();
                return Ok(new { TotalMarketValue = totalValue });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetTotalMarketValue da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Stock statistikasi
        /// </summary>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStockStatistics()
        {
            try
            {
                var statistics = await _stockService.GetStockStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStockStatistics da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Price rangelari bo'yicha statistika
        /// </summary>
        [HttpGet("price-ranges")]
        public async Task<IActionResult> GetStocksByPriceRanges()
        {
            try
            {
                var ranges = await _stockService.GetStocksByPriceRangesAsync();
                return Ok(ranges);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksByPriceRanges da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        #endregion

        #region Date-based Queries

        /// <summary>
        /// Sana oralig'ida yangilangan stocklar
        /// </summary>
        [HttpGet("updated-in-range")]
        public async Task<IActionResult> GetStocksUpdatedInRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            try
            {
                var stocks = await _stockService.GetStocksUpdatedInRangeAsync(startDate, endDate);
                return Ok(stocks);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksUpdatedInRange da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Bugun yangilangan stocklar
        /// </summary>
        [HttpGet("updated-today")]
        public async Task<IActionResult> GetStocksUpdatedToday()
        {
            try
            {
                var stocks = await _stockService.GetStocksUpdatedTodayAsync();
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksUpdatedToday da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Bu hafta yangilangan stocklar
        /// </summary>
        [HttpGet("updated-this-week")]
        public async Task<IActionResult> GetStocksUpdatedThisWeek()
        {
            try
            {
                var stocks = await _stockService.GetStocksUpdatedThisWeekAsync();
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksUpdatedThisWeek da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Bu oy yangilangan stocklar
        /// </summary>
        [HttpGet("updated-this-month")]
        public async Task<IActionResult> GetStocksUpdatedThisMonth()
        {
            try
            {
                var stocks = await _stockService.GetStocksUpdatedThisMonthAsync();
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksUpdatedThisMonth da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        #endregion

        #region Grouping Operations

        /// <summary>
        /// Narx bo'yicha guruhlangan stocklar
        /// </summary>
        [HttpGet("grouped-by-price")]
        public async Task<IActionResult> GetStocksGroupedByPriceRange()
        {
            try
            {
                var groups = await _stockService.GetStocksGroupedByPriceRangeAsync();
                return Ok(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksGroupedByPriceRange da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Birinchi harf bo'yicha stock sonlari
        /// </summary>
        [HttpGet("count-by-first-letter")]
        public async Task<IActionResult> GetStockCountByFirstLetter()
        {
            try
            {
                var letterCounts = await _stockService.GetStockCountByFirstLetterAsync();
                return Ok(letterCounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStockCountByFirstLetter da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        #endregion

        #region Related Data Operations

        /// <summary>
        /// Stocklar comment sonlari bilan
        /// </summary>
        [HttpGet("with-comment-counts")]
        public async Task<IActionResult> GetStocksWithCommentCount()
        {
            try
            {
                var stocksWithComment = await _stockService.GetStocksWithCommentCountAsync();
                return Ok(stocksWithComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksWithCommentCount da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Eng ko'p commentli stocklar
        /// </summary>
        [HttpGet("most-commented")]
        public async Task<IActionResult> GetStocksWithMostComment([FromQuery] int count = 5)
        {
            try
            {
                var stocks = await _stockService.GetStocksWithMostCommentAsync(count);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksWithMostComment da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Commenti yo'q stocklar
        /// </summary>
        [HttpGet("without-Comment")]
        public async Task<IActionResult> GetStocksWithoutComment()
        {
            try
            {
                var stocks = await _stockService.GetStocksWithoutCommentAsync();
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksWithoutComment da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        #endregion

        #region Advanced Operations

        /// <summary>
        /// Narxlarni bulk update qilish
        /// </summary>
        [HttpPut("bulk-update-prices")]
        public async Task<IActionResult> BulkUpdatePrices([FromBody] Dictionary<int, decimal> priceUpdates)
        {
            try
            {
                if (priceUpdates == null || !priceUpdates.Any())
                    return BadRequest("Price updates bo'sh bo'lishi mumkin emas");

                var success = await _stockService.BulkUpdatePricesAsync(priceUpdates);
                if (!success)
                    return BadRequest("Hech qanday stock yangilanmadi");

                return Ok(new { Message = $"{priceUpdates.Count} ta stock narxi yangilandi", Success = success });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BulkUpdatePrices da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Raw SQL bajarish (faqat SELECT)
        /// </summary>
        [HttpPost("raw-sql")]
        public async Task<IActionResult> ExecuteRawSql([FromBody] RawSqlRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Sql))
                    return BadRequest("SQL query bo'sh bo'lishi mumkin emas");

                var stocks = await _stockService.ExecuteRawSqlAsync(request.Sql, request.Parameters ?? Array.Empty<object>());
                return Ok(stocks);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ExecuteRawSql da xato");
                return BadRequest("SQL query xato: " + ex.Message);
            }
        }

        /// <summary>
        /// SQL WHERE clause bilan count
        /// </summary>
        [HttpPost("count-by-sql")]
        public async Task<IActionResult> GetStockCountBySql([FromBody] WhereClauseRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.WhereClause))
                    return BadRequest("WHERE clause bo'sh bo'lishi mumkin emas");

                var count = await _stockService.GetStockCountBySqlAsync(request.WhereClause);
                return Ok(new { Count = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStockCountBySql da xato");
                return BadRequest("SQL query xato: " + ex.Message);
            }
        }

        #endregion

        #region Performance & Utility

        /// <summary>
        /// No-tracking bilan stocklar (performance optimized)
        /// </summary>
        [HttpGet("no-tracking")]
        public async Task<IActionResult> GetStocksAsNoTracking()
        {
            try
            {
                var stocks = await _stockService.GetStocksAsNoTrackingAsync();
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStocksAsNoTracking da xato");
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Stock mavjudligini tekshirish (ID bo'yicha)
        /// </summary>
        [HttpGet("{id:int}/exists")]
        public async Task<IActionResult> CheckStockExists([FromRoute] int id)
        {
            try
            {
                var exists = await _stockService.StockExistsAsync(id);
                return Ok(new { Id = id, Exists = exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckStockExists da xato: {Id}", id);
                return StatusCode(500, "Server xatosi");
            }
        }

        /// <summary>
        /// Stock mavjudligini tekshirish (Symbol bo'yicha)
        /// </summary>
        [HttpGet("symbol/{symbol}/exists")]
        public async Task<IActionResult> CheckStockExistsBySymbol([FromRoute] string symbol)
        {
            try
            {
                var exists = await _stockService.StockExistsBySymbolAsync(symbol);
                return Ok(new { Symbol = symbol, Exists = exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckStockExistsBySymbol da xato: {Symbol}", symbol);
                return StatusCode(500, "Server xatosi");
            }
        }

        #endregion
    }

    #region Helper DTOs

    /// <summary>
    /// Raw SQL so'rov uchun DTO
    /// </summary>
    public class RawSqlRequest
    {
        public string Sql { get; set; } = string.Empty;
        public object[]? Parameters { get; set; }
    }

    /// <summary>
    /// WHERE clause so'rov uchun DTO
    /// </summary>
    public class WhereClauseRequest
    {
        public string WhereClause { get; set; } = string.Empty;
    }

    #endregion
}
using api.DTOs;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;

namespace api.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<StockService> _logger;

        public StockService(IStockRepository stockRepository, ILogger<StockService> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        // Basic CRUD Operations
        public async Task<List<StockDto>> GetAllStocksAsync()
        {
            try
            {
                _logger.LogInformation("Barcha stocklarni olish boshlandi");
                var stocks = await _stockRepository.GetAllAsync();
                var result = stocks.Select(stock => stock.ToStockDto()).ToList();
                _logger.LogInformation("Muvaffaqiyatli {Count} ta stock olindi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Barcha stocklarni olishda xato");
                throw;
            }
        }

        public async Task<StockDto?> GetStockByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Noto'g'ri Stock ID: {Id}", id);
                    return null;
                }

                _logger.LogInformation("Stock olish boshlandi, ID: {Id}", id);
                var stock = await _stockRepository.GetByIdAsync(id);
                
                if (stock == null)
                {
                    _logger.LogWarning("Stock topilmadi, ID: {Id}", id);
                    return null;
                }

                return stock.ToStockDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stock olishda xato, ID: {Id}", id);
                throw;
            }
        }

        public async Task<StockDto> CreateStockAsync(CreateStockDTO createStockDto)
        {
            try
            {
                // Input validation
                if (createStockDto == null)
                    throw new ArgumentNullException(nameof(createStockDto), "CreateStockDTO bo'sh bo'lishi mumkin emas");

                if (string.IsNullOrWhiteSpace(createStockDto.Symbol))
                    throw new ArgumentException("Stock symbol bo'sh bo'lishi mumkin emas");

                if (createStockDto.Price <= 0)
                    throw new ArgumentException("Price 0 dan katta bo'lishi kerak");

                if (string.IsNullOrWhiteSpace(createStockDto.CompanyName))
                    throw new ArgumentException("Company name bo'sh bo'lishi mumkin emas");

                // Check if symbol already exists
                var existingStock = await _stockRepository.GetBySymbolAsync(createStockDto.Symbol);
                if (existingStock != null)
                    throw new InvalidOperationException($"Symbol '{createStockDto.Symbol}' allaqachon mavjud");

                _logger.LogInformation("Yangi stock yaratish boshlandi: {Symbol}", createStockDto.Symbol);
                
                var stockModel = createStockDto.ToStockFromCreateDTO();
                var createdStock = await _stockRepository.CreateAsync(stockModel);

                _logger.LogInformation("Yangi stock muvaffaqiyatli yaratildi: {Symbol}, ID: {Id}", 
                    createdStock.Symbol, createdStock.Id);

                return createdStock.ToStockDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stock yaratishda xato: {Symbol}", createStockDto?.Symbol);
                throw;
            }
        }

        public async Task<StockDto?> UpdateStockAsync(int id, UpdateStockDTO updateStockDto)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Noto'g'ri Stock ID: {Id}", id);
                    return null;
                }

                if (updateStockDto == null)
                    throw new ArgumentNullException(nameof(updateStockDto), "UpdateStockDTO bo'sh bo'lishi mumkin emas");

                // Price validation
                if (updateStockDto.Price.HasValue && updateStockDto.Price <= 0)
                    throw new ArgumentException("Price 0 dan katta bo'lishi kerak");

                // Check if symbol already exists (if updating symbol)
                if (!string.IsNullOrWhiteSpace(updateStockDto.Symbol))
                {
                    var existingStock = await _stockRepository.GetBySymbolAsync(updateStockDto.Symbol);
                    if (existingStock != null && existingStock.Id != id)
                        throw new InvalidOperationException($"Symbol '{updateStockDto.Symbol}' boshqa stockda mavjud");
                }

                _logger.LogInformation("Stock yangilash boshlandi, ID: {Id}", id);
                
                var updatedStock = await _stockRepository.UpdateAsync(id, updateStockDto);
                
                if (updatedStock == null)
                {
                    _logger.LogWarning("Yangilanishi kerak bo'lgan stock topilmadi, ID: {Id}", id);
                    return null;
                }

                _logger.LogInformation("Stock muvaffaqiyatli yangilandi, ID: {Id}", id);
                return updatedStock.ToStockDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stock yangilashda xato, ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteStockAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Noto'g'ri Stock ID: {Id}", id);
                    return false;
                }

                _logger.LogInformation("Stock o'chirish boshlandi, ID: {Id}", id);
                
                var deletedStock = await _stockRepository.DeleteAsync(id);

                if (deletedStock != null)
                {
                    _logger.LogInformation("Stock muvaffaqiyatli o'chirildi, ID: {Id}, Symbol: {Symbol}", 
                        id, deletedStock.Symbol);
                    return true;
                }

                _logger.LogWarning("O'chirilishi kerak bo'lgan stock topilmadi, ID: {Id}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stock o'chirishda xato, ID: {Id}", id);
                throw;
            }
        }

        // Complex Queries
        public async Task<StockDto?> GetStockBySymbolAsync(string symbol)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(symbol))
                {
                    _logger.LogWarning("Symbol bo'sh yoki null");
                    return null;
                }

                _logger.LogInformation("Stock symbol bo'yicha qidirilmoqda: {Symbol}", symbol);
                
                var stock = await _stockRepository.GetBySymbolAsync(symbol.Trim());
                
                if (stock == null)
                {
                    _logger.LogInformation("Symbol bo'yicha stock topilmadi: {Symbol}", symbol);
                    return null;
                }

                return stock.ToStockDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Symbol bo'yicha stock qidirishda xato: {Symbol}", symbol);
                throw;
            }
        }

        public async Task<List<StockDto>> GetStocksByCompanyNameAsync(string companyName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    return new List<StockDto>();

                _logger.LogInformation("Company name bo'yicha stocklar qidirilmoqda: {CompanyName}", companyName);
                
                var stocks = await _stockRepository.GetByCompanyNameAsync(companyName.Trim());
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("{Count} ta stock topildi company name bo'yicha", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Company name bo'yicha qidirishda xato: {CompanyName}", companyName);
                throw;
            }
        }

        public async Task<List<StockDto>> GetStocksWithCommentAsync()
        {
            try
            {
                _logger.LogInformation("Comment mavjud stocklar olish boshlandi");
                
                var stocks = await _stockRepository.GetStocksWithCommentAsync();
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("{Count} ta comment mavjud stock topildi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Comment mavjud stocklarni olishda xato");
                throw;
            }
        }

        public async Task<StockDto?> GetStockWithCommentAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Noto'g'ri Stock ID: {Id}", id);
                    return null;
                }

                _logger.LogInformation("Stock commentlari bilan olish boshlandi, ID: {Id}", id);
                
                var stock = await _stockRepository.GetStockWithCommentAsync(id);
                
                if (stock == null)
                {
                    _logger.LogWarning("Stock topilmadi, ID: {Id}", id);
                    return null;
                }

                return stock.ToStockDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stock commentlari bilan olishda xato, ID: {Id}", id);
                throw;
            }
        }

        public async Task<List<StockDto>> GetStocksByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            try
            {
                if (minPrice < 0 || maxPrice < 0)
                    throw new ArgumentException("Price manfiy bo'lishi mumkin emas");

                if (minPrice > maxPrice)
                    throw new ArgumentException("MinPrice MaxPrice dan katta bo'lishi mumkin emas");

                _logger.LogInformation("Price range bo'yicha stocklar olish: {MinPrice} - {MaxPrice}", minPrice, maxPrice);
                
                var stocks = await _stockRepository.GetStocksByPriceRangeAsync(minPrice, maxPrice);
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("{Count} ta stock topildi price range da", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Price range bo'yicha qidirishda xato: {MinPrice} - {MaxPrice}", minPrice, maxPrice);
                throw;
            }
        }

        public async Task<List<StockDto>> GetTopExpensiveStocksAsync(int count)
        {
            try
            {
                if (count <= 0)
                    count = 10;
                if (count > 100)
                    count = 100;

                _logger.LogInformation("Eng qimmat {Count} ta stock olish boshlandi", count);
                
                var stocks = await _stockRepository.GetTopExpensiveStocksAsync(count);
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("{ResultCount} ta eng qimmat stock olindi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eng qimmat stocklarni olishda xato");
                throw;
            }
        }

        public async Task<List<StockDto>> GetRecentlyUpdatedStocksAsync(int days)
        {
            try
            {
                if (days <= 0)
                    days = 7;
                if (days > 365)
                    days = 365;

                _logger.LogInformation("So'nggi {Days} kun ichida yangilangan stocklar olish", days);
                
                var stocks = await _stockRepository.GetRecentlyUpdatedStocksAsync(days);
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("{Count} ta yaqinda yangilangan stock topildi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yaqinda yangilangan stocklarni olishda xato");
                throw;
            }
        }

        // Filtering & Sorting
        public async Task<List<StockDto>> GetFilteredStocksAsync(StockFilterDto filter)
        {
            try
            {
                if (filter == null)
                    return await GetAllStocksAsync();

                _logger.LogInformation("Filtr bilan stocklar olish boshlandi");
                
                var stocks = await _stockRepository.GetFilteredStocksAsync(filter);
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("Filtr bo'yicha {Count} ta stock topildi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Filtr bo'yicha qidirishda xato");
                throw;
            }
        }

        public async Task<List<StockDto>> GetSortedStocksAsync(string sortBy, bool descending = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sortBy))
                    sortBy = "Id";

                _logger.LogInformation("Stocklar tartibga solinmoqda: {SortBy}, Descending: {Descending}", sortBy, descending);
                
                var stocks = await _stockRepository.GetSortedStocksAsync(sortBy, descending);
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("{Count} ta stock tartibga solindi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stocklarni tartibga solishda xato");
                throw;
            }
        }

        public async Task<PagedResultDto<StockDto>> GetPagedStocksAsync(int pageNumber, int pageSize, string sortBy = "Id")
        {
            try
            {
                // Input validation
                if (pageNumber <= 0) pageNumber = 1;
                if (pageSize <= 0) pageSize = 10;
                if (pageSize > 100) pageSize = 100;
                if (string.IsNullOrWhiteSpace(sortBy)) sortBy = "Id";

                _logger.LogInformation("Sahifalangan stocklar olish: Page {PageNumber}, Size {PageSize}, Sort {SortBy}", 
                    pageNumber, pageSize, sortBy);

                var (stocks, totalCount) = await _stockRepository.GetPagedStocksAsync(pageNumber, pageSize, sortBy);
                var stockDtos = stocks.Select(s => s.ToStockDto()).ToList();
                
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var result = new PagedResultDto<StockDto>
                {
                    Data = stockDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < totalPages
                };

                _logger.LogInformation("Sahifalangan natija: {Count} ta stock, Jami {TotalCount}", 
                    stockDtos.Count, totalCount);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sahifalangan stocklarni olishda xato");
                throw;
            }
        }

        // Search Operations
        public async Task<List<StockDto>> SearchStocksAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return new List<StockDto>();

                _logger.LogInformation("Stock qidirish boshlandi: {SearchTerm}", searchTerm);
                
                var stocks = await _stockRepository.SearchStocksAsync(searchTerm.Trim());
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("Qidiruv bo'yicha {Count} ta stock topildi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stock qidirishda xato: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<List<StockDto>> SearchStocksBySymbolAsync(string symbolPattern)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(symbolPattern))
                    return new List<StockDto>();

                _logger.LogInformation("Symbol pattern bo'yicha qidirish: {Pattern}", symbolPattern);
                
                var stocks = await _stockRepository.SearchStocksBySymbolAsync(symbolPattern);
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("Symbol pattern bo'yicha {Count} ta stock topildi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Symbol pattern bo'yicha qidirishda xato: {Pattern}", symbolPattern);
                throw;
            }
        }

        // Aggregation Operations
        public async Task<int> GetTotalStockCountAsync()
        {
            try
            {
                var count = await _stockRepository.GetTotalStockCountAsync();
                _logger.LogInformation("Jami stock soni: {Count}", count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Jami stock sonini olishda xato");
                throw;
            }
        }

        public async Task<decimal> GetAverageStockPriceAsync()
        {
            try
            {
                var avgPrice = await _stockRepository.GetAverageStockPriceAsync();
                _logger.LogInformation("O'rtacha stock narxi: {AvgPrice:C}", avgPrice);
                return avgPrice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "O'rtacha stock narxini olishda xato");
                throw;
            }
        }

        public async Task<decimal> GetTotalMarketValueAsync()
        {
            try
            {
                var totalValue = await _stockRepository.GetTotalMarketValueAsync();
                _logger.LogInformation("Jami bozor qiymati: {TotalValue:C}", totalValue);
                return totalValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Jami bozor qiymatini olishda xato");
                throw;
            }
        }

        public async Task<StockStatisticsDto> GetStockStatisticsAsync()
        {
            try
            {
                _logger.LogInformation("Stock statistikasi olish boshlandi");
                var statistics = await _stockRepository.GetStockStatisticsAsync();
                _logger.LogInformation("Stock statistikasi muvaffaqiyatli olindi");
                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stock statistikasini olishda xato");
                throw;
            }
        }

        public async Task<List<StocksByPriceRangeDto>> GetStocksByPriceRangesAsync()
        {
            try
            {
                _logger.LogInformation("Price rangelari bo'yicha stocklar olish");
                var ranges = await _stockRepository.GetStocksByPriceRangesAsync();
                _logger.LogInformation("{Count} ta price range topildi", ranges.Count);
                return ranges;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Price rangelari bo'yicha olishda xato");
                throw;
            }
        }

        // Date-based Queries
        public async Task<List<StockDto>> GetStocksUpdatedInRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    throw new ArgumentException("Start date end date dan kichik bo'lishi kerak");

                _logger.LogInformation("Sana oralig'ida yangilangan stocklar: {StartDate} - {EndDate}", 
                    startDate, endDate);
                
                var stocks = await _stockRepository.GetStocksUpdatedInRangeAsync(startDate, endDate);
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("Sana oralig'ida {Count} ta stock topildi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sana oralig'ida qidirishda xato");
                throw;
            }
        }

        public async Task<List<StockDto>> GetStocksUpdatedTodayAsync()
        {
            try
            {
                _logger.LogInformation("Bugun yangilangan stocklar olish");
                var stocks = await _stockRepository.GetStocksUpdatedTodayAsync();
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                _logger.LogInformation("Bugun {Count} ta stock yangilangan", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bugun yangilangan stocklarni olishda xato");
                throw;
            }
        }

        public async Task<List<StockDto>> GetStocksUpdatedThisWeekAsync()
        {
            try
            {
                _logger.LogInformation("Bu hafta yangilangan stocklar olish");
                var stocks = await _stockRepository.GetStocksUpdatedThisWeekAsync();
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                _logger.LogInformation("Bu hafta {Count} ta stock yangilangan", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bu hafta yangilangan stocklarni olishda xato");
                throw;
            }
        }

        public async Task<List<StockDto>> GetStocksUpdatedThisMonthAsync()
        {
            try
            {
                _logger.LogInformation("Bu oy yangilangan stocklar olish");
                var stocks = await _stockRepository.GetStocksUpdatedThisMonthAsync();
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                _logger.LogInformation("Bu oy {Count} ta stock yangilangan", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bu oy yangilangan stocklarni olishda xato");
                throw;
            }
        }

        // Grouping Operations
        public async Task<List<StockGroupDto>> GetStocksGroupedByPriceRangeAsync()
        {
            try
            {
                _logger.LogInformation("Narx bo'yicha guruhlangan stocklar olish");
                var groups = await _stockRepository.GetStocksGroupedByPriceRangeAsync();
                _logger.LogInformation("{Count} ta narx guruhi topildi", groups.Count);
                return groups;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Narx bo'yicha guruhlashda xato");
                throw;
            }
        }

        public async Task<Dictionary<string, int>> GetStockCountByFirstLetterAsync()
        {
            try
            {
                _logger.LogInformation("Birinchi harf bo'yicha stock soni");
                var letterCounts = await _stockRepository.GetStockCountByFirstLetterAsync();
                _logger.LogInformation("{Count} ta harf bo'yicha guruhlandi", letterCounts.Count);
                return letterCounts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Harf bo'yicha guruhlashda xato");
                throw;
            }
        }

        // Related Data Operations
        public async Task<List<StockWithCommentDto>> GetStocksWithCommentCountAsync()
        {
            try
            {
                _logger.LogInformation("Stocklar comment sonlari bilan olish");
                var stocksWithComment = await _stockRepository.GetStocksWithCommentCountAsync();
                _logger.LogInformation("{Count} ta stock comment sonlari bilan olindi", stocksWithComment.Count);
                return stocksWithComment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Comment sonlari bilan olishda xato");
                throw;
            }
        }

        public async Task<List<StockDto>> GetStocksWithMostCommentAsync(int count)
        {
            try
            {
                if (count <= 0) count = 5;
                if (count > 50) count = 50;

                _logger.LogInformation("Eng ko'p commentli {Count} ta stock olish", count);
                var stocks = await _stockRepository.GetStocksWithMostCommentAsync(count);
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                _logger.LogInformation("{ResultCount} ta eng ko'p commentli stock olindi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eng ko'p commentli stocklarni olishda xato");
                throw;
            }
        }

        public async Task<List<StockDto>> GetStocksWithoutCommentAsync()
        {
            try
            {
                _logger.LogInformation("Commenti yo'q stocklar olish");
                var stocks = await _stockRepository.GetStocksWithoutCommentAsync();
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                _logger.LogInformation("Commenti yo'q {Count} ta stock topildi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commenti yo'q stocklarni olishda xato");
                throw;
            }
        }

        // Advanced Operations
        public async Task<bool> BulkUpdatePricesAsync(Dictionary<int, decimal> priceUpdates)
        {
            try
            {
                if (priceUpdates == null || !priceUpdates.Any())
                {
                    _logger.LogWarning("Price updates bo'sh yoki null");
                    return false;
                }

                // Validation
                foreach (var (id, price) in priceUpdates)
                {
                    if (id <= 0)
                        throw new ArgumentException($"Noto'g'ri Stock ID: {id}");
                    if (price <= 0)
                        throw new ArgumentException($"Noto'g'ri Price: {price} for Stock ID: {id}");
                }

                _logger.LogInformation("Bulk price update boshlandi: {Count} ta stock", priceUpdates.Count);
                
                var result = await _stockRepository.BulkUpdatePricesAsync(priceUpdates);
                
                if (result)
                    _logger.LogInformation("Bulk price update muvaffaqiyatli bajarildi");
                else
                    _logger.LogWarning("Bulk price update muvaffaqiyatsiz");
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bulk price update da xato");
                throw;
            }
        }

        public async Task<List<StockDto>> ExecuteRawSqlAsync(string sql, params object[] parameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sql))
                    throw new ArgumentException("SQL query bo'sh bo'lishi mumkin emas");

                // Security check - faqat SELECT querylar
                var trimmedSql = sql.Trim().ToUpper();
                if (!trimmedSql.StartsWith("SELECT"))
                    throw new ArgumentException("Faqat SELECT querylar ruxsat etilgan");

                _logger.LogWarning("Raw SQL bajarilmoqda - ehtiyotkorlik bilan ishlating");
                
                var stocks = await _stockRepository.ExecuteRawSqlAsync(sql, parameters);
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("Raw SQL natijasi: {Count} ta stock", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Raw SQL bajarishda xato");
                throw;
            }
        }

        public async Task<int> GetStockCountBySqlAsync(string whereClause)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(whereClause))
                    return await GetTotalStockCountAsync();

                _logger.LogInformation("SQL WHERE clause bilan count: {WhereClause}", whereClause);
                
                var count = await _stockRepository.GetStockCountBySqlAsync(whereClause);
                
                _logger.LogInformation("SQL count natijasi: {Count}", count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SQL count da xato");
                throw;
            }
        }

        // Performance & Utility
        public async Task<List<StockDto>> GetStocksAsNoTrackingAsync()
        {
            try
            {
                _logger.LogInformation("No-tracking bilan stocklar olish (performance optimized)");
                
                var stocks = await _stockRepository.GetStocksAsNoTrackingAsync();
                var result = stocks.Select(s => s.ToStockDto()).ToList();
                
                _logger.LogInformation("No-tracking bilan {Count} ta stock olindi", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No-tracking stocklarni olishda xato");
                throw;
            }
        }

        public async Task<bool> StockExistsAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Noto'g'ri Stock ID mavjudlik tekshiruvi: {Id}", id);
                    return false;
                }

                var exists = await _stockRepository.ExistsAsync(id);
                _logger.LogInformation("Stock mavjudlik tekshiruvi ID {Id}: {Exists}", id, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stock mavjudligini tekshirishda xato, ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> StockExistsBySymbolAsync(string symbol)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(symbol))
                {
                    _logger.LogWarning("Bo'sh symbol mavjudlik tekshiruvi");
                    return false;
                }

                var exists = await _stockRepository.ExistsBySymbolAsync(symbol.Trim());
                _logger.LogInformation("Stock symbol mavjudlik tekshiruvi {Symbol}: {Exists}", symbol, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Symbol mavjudligini tekshirishda xato: {Symbol}", symbol);
                throw;
            }
        }
    }
}
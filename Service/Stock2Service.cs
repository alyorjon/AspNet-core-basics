
// using api.DTOs;
// using api.DTOs.Stock;
// using api.Interfaces.Stock2;

// namespace api.Service
// {
//     public class Stock2Service : IStock2Service
//     {
//         private readonly IStock2Repository _stockRepository;
//         public Stock2Service(IStock2Repository stockRepository)
//         {
//             _stockRepository = stockRepository;
//         }

//         public Task<List<StockDto>> GetAllStock()
//         {
//             throw new NotImplementedException();
//         }

//         public Task<StockDto> GetById(int id)
//         {
//             throw new NotImplementedException();
//         }
//         public async Task<StockDto> CreateStock(CreateStockDTO createStockDTO)
//         {
//             var stockModel = await _stockRepository.CreateStock(createStockDTO);
//             return stockModel.ToStockDto();
//         }
//         public Task<StockDto> UpdateStock(int id, UpdateStockDTO updateStockDTO)
//         {
//             throw new NotImplementedException();
//         }
        

//         public Task<StockDto> DeleteById(int id)
//         {
//             throw new NotImplementedException();
//         }

//     }
// }
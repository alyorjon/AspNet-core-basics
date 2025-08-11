// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using api.DTOs.Stock;
// using api.Interfaces;
// using api.Interfaces.Stock2;
// using api.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class Stock2Controller : ControllerBase
//     {
//         #region Constructor
//         private readonly IStock2Service _stockService;
//         private readonly ILogger<CommentController> _logger;
//         public Stock2Controller(IStock2Service stockService,
//                                 ILogger<CommentController> logger)
//         {
//             _stockService = stockService;
//             _logger = logger;
//         }
//         #endregion Constructor
//         #region  Get All comments
//         [HttpGet]
//         public async Task<IActionResult> GetAllAsync()
//         {
//             var commentDto = await _stockService.GetAllStock();
//             return Ok(commentDto);
//             // throw new NotImplementedException();
//         }
//         #endregion
//         #region  Get By ID 
//         [HttpGet("{id:int}")]
//         public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
//         {
//             var commentDto = await _stockService.GetById(id);
//             return Ok(commentDto);
//             // throw new NotImplementedException();
//         }
//         #endregion Get By ID

//         #region  Create New Stock
//         [HttpPost]
//         public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO createStockDTO)
//         {
//             var commentDto = await _stockService.CreateStock(createStockDTO);
//             return Ok(commentDto);
//             // throw new NotImplementedException();
//         }
//         #endregion Create New Stock
//         #region  Patch Stock by ID
//         [HttpPatch("{id:int}")]
//         public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDTO updateStockDTO)
//         {
//             var commentDto = await _stockService.UpdateStock(id, updateStockDTO);
//             return Ok(commentDto);
//             // throw new NotImplementedException();
//         }
//         #endregion Patch Stock by ID

//         #region  Delete Stock by ID
//         [HttpDelete("{id:int}")]
//         public async Task<IActionResult> DeleteStock([FromRoute] int id)
//         {
//             var commentDto = await _stockService.DeleteById(id);
//             return Ok(commentDto);
//             // throw new NotImplementedException();
//         }
//         #endregion Delete Stock by ID

//     }
// }
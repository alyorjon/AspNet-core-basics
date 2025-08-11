using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.ApplicationDbContext;
using api.DTOs.Book;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ApplicationDBContext _context;
        public BookController(IBookRepository bookRepository, ApplicationDBContext context)
        {
            _context = context;
            _bookRepository = bookRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            return Ok(books.Select(book => book.ToBookDTO()));
            // return 
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute] Guid id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book.ToBookDTO());
        }

        [HttpPost]
        public async Task<IActionResult> InsertBook([FromBody] CreateBookDto bookDTO)
        {
            var bookModel = bookDTO.ToCreateBookDto();
            await _bookRepository.CreateBookAsync(bookModel);
            return Ok(bookModel.ToBookDTO());
        }
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateBook([FromRoute] Guid id, [FromBody] UpdateBookDto updateBookDto)
        {
            var bookModel = await _bookRepository.UpdateBookAsync(id, updateBookDto);
            if (bookModel == null)
            {
                return NotFound();
            }
            return Ok(bookModel.ToBookDTO());
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBookByIdAsync([FromRoute] Guid id)
        {
            var bookModel = await _bookRepository.DeleteBookByIdAsync(id);
            if (bookModel == null)
            {
                return NotFound();
            }
            return Ok(bookModel.ToBookDTO());
        }

    }
}
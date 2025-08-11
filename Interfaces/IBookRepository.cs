using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Book;
using api.Models;

namespace api.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(Guid id);
        Task<Book> GetBookByTitleAsync(string title);
        Task<Book> CreateBookAsync(Book bookModel);
        Task<Book> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto);
        Task<Book> DeleteBookByIdAsync(Guid id);
    }
}
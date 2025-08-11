
using api.ApplicationDbContext;
using api.DTOs.Book;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class BookRepository : IBookRepository
    {
        private  readonly ApplicationDBContext _context;
        public BookRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Book.Where(x => x.IsActive == true).OrderBy(x => x.title).ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _context.Book.FirstOrDefaultAsync(x => x.id == id);
            
        }

        public async Task<Book> GetBookByTitleAsync(string title)
        {
            return await _context.Book.FirstOrDefaultAsync(x => x.title == title);
        }

        public async Task<Book> CreateBookAsync(Book bookModel)
        {
            var book = new Book
            {
                id = bookModel.id,
                title = bookModel.title,
                genre = bookModel.genre,
                writer = bookModel.writer,
                description = bookModel.description,
                publishedAt = bookModel.publishedAt,
                likes = bookModel.likes
            };
            await _context.Book.AddAsync(book);
            await _context.SaveChangesAsync();
            return book; 
        }




        public async Task<Book> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto)
        {
            var book = await _context.Book.FirstOrDefaultAsync(x => x.id == id);
            if (book == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(updateBookDto.title))
                book.title = updateBookDto.title;
            
            if (!string.IsNullOrEmpty(updateBookDto.genre))
                book.genre = updateBookDto.genre;
            if (!string.IsNullOrEmpty(updateBookDto.writer))
                book.writer = updateBookDto.writer;
            if (!string.IsNullOrEmpty(updateBookDto.description))
                book.description = updateBookDto.description;
            if (updateBookDto.publishedAt != DateTime.MinValue)

                book.publishedAt = updateBookDto.publishedAt;
           
            if (updateBookDto.likes != 0)
                    book.likes = updateBookDto.likes;
            
            await _context.SaveChangesAsync();
            return book;
        }
        public async Task<Book> DeleteBookByIdAsync(Guid id)
        {
            var book = await _context.Book.FirstOrDefaultAsync(x => x.id == id);
            if (book == null)
            {
                return null;
            }
            book.IsActive = false;
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
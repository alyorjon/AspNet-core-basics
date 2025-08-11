
using api.DTOs.Book;
using api.Models;

namespace api.Mappers
{
    public static class BookMappers
    {
        public static BookDto ToBookDTO(this Book bookModel)
        {
            return new BookDto
            {
                id = bookModel.id,
                title = bookModel.title,
                genre = bookModel.genre,
                writer = bookModel.writer,
                description = bookModel.description,
                publishedAt = bookModel.publishedAt,
                likes = bookModel.likes,
                IsActive = bookModel.IsActive
            };

        }
        public static Book ToCreateBookDto(this CreateBookDto createBookDto)
        {
            return new Book
            {
                title = createBookDto.title,
                genre = createBookDto.genre,
                writer = createBookDto.writer,
                description = createBookDto.description,
                publishedAt = createBookDto.publishedAt,
                likes = createBookDto.likes
            };
        }
    }
}
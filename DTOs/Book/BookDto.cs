

namespace api.DTOs.Book
{
    public class BookDto
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string genre { get; set; }
        public string writer { get; set; }
        public string description { get; set; }
        public DateTimeOffset publishedAt { get; set; }
        public int likes { get; set; } 
        public bool IsActive { get; set; } = true;
    }
}
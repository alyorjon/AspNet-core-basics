using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Book
{
    public class CreateBookDto
    {
        // public Guid id { get; set; }
        public string title { get; set; }
        public string genre { get; set; }
        public string writer { get; set; }
        public string description { get; set; }
        public DateTimeOffset publishedAt { get; set; }
        public int likes { get; set; }
    }
}
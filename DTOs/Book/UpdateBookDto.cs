using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Book
{
    public class UpdateBookDto
    {
        public string title { get; set; }
        public string genre { get; set; }=string.Empty;
        public string writer { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public DateTime publishedAt { get; set; }
        public int likes { get; set; } = default(int);
    }
}
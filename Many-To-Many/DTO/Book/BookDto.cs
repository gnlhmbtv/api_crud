using Many_To_Many.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Many_To_Many.DTO.Book
{
    public class BookDto
    {
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
    }
}

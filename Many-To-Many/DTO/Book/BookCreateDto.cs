using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Many_To_Many.DTO.Book
{
    public class BookCreateDto
    {
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public List<int> GenresId { get; set; }
    }
}

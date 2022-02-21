using Many_To_Many.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Many_To_Many.Data.Entity
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public List<BookGenre> BookGenres { get; set; }
    }
}

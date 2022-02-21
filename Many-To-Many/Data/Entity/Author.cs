using Many_To_Many.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Many_To_Many.Data.Entity
{
    public class Author: BaseEntity
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}

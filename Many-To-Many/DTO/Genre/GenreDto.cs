using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Many_To_Many.DTO.Genre
{
    public class GenreDto
    {
        public string Name { get; set; }
        public List<string> Books { get; set; }
    }
}

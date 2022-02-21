using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Many_To_Many.Data.DAL;
using Many_To_Many.Data.Entity;
using Many_To_Many.DTO.Book;

namespace Many_To_Many.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly Context _context;

        public BooksController(Context context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            string authors = await _context.Add(authors);
            List<BookDto> bookDtos = new List<BookDto>();
            var books = await _context.Books.Include(b => b.Author).Include(b => b.BookGenres).ThenInclude(b => b.Genre).ToListAsync();

            foreach (var item in books)
            {
                BookDto bookDto = new BookDto
                {
                    Name = item.Name,
                    AuthorName = item.Author.Name,
                    Genres = item.BookGenres.Select(b => b.Genre.Name).ToList()
                };
                bookDtos.Add(bookDto);
            }

            return bookDtos;
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(b => b.Genre)
                .FirstOrDefaultAsync(b => b.Id == id);

            BookDto bookDtos = new BookDto()
            {
                Name = book.Name,
                AuthorName = book.Author.Name,
                Genres = book.BookGenres.Select(b => b.Genre.Name).ToList()
            };

            if (book == null)
            {
                return NotFound();
            }

            return bookDtos;
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookCreateDto bookDto)
        {
            Book book = new Book()
            {
                Name = bookDto.Name,
                AuthorId = bookDto.AuthorId,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            foreach (var item in bookDto.GenresId)
            {
                BookGenre bookGenre = new BookGenre()
                {
                    GenreId = item,
                    BookId=book.Id
                };

                _context.BookGenres.Add(bookGenre);
                await _context.SaveChangesAsync();
            }

             return Ok(await GetBook(book.Id));
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}

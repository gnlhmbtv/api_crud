using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Many_To_Many.Data.DAL;
using Many_To_Many.Data.Entity;
using Many_To_Many.DTO.Genre;

namespace Many_To_Many.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly Context _context;

        public GenresController(Context context)
        {
            _context = context;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
        {
            List<GenreDto> genreDtos = new List<GenreDto>();
            var genres = await _context.Genres.Include(b => b.BookGenres).ThenInclude(b => b.Book).ToListAsync();

            foreach (var item in genres)
            {
                GenreDto genreDto = new GenreDto
                {
                    Name = item.Name,
                    Books = item.BookGenres.Select(b => b.Book.Name).ToList()
                };
                genreDtos.Add(genreDto);
            }

            return genreDtos;
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDto>> GetGenre(int id)
        {
            var genre = await _context.Genres
                .Include(b => b.BookGenres)
                .ThenInclude(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == id);

            GenreDto genreDto= new GenreDto()
            {
                Name = genre.Name,
                Books = genre.BookGenres.Select(b => b.Book.Name).ToList()
            };

            if (genre == null)
            {
                return NotFound();
            }

            return genreDto;
        }

        // PUT: api/Genres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, Genre genre)
        {
            if (id != genre.Id)
            {
                return BadRequest();
            }

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, genre);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}

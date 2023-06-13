using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Laba_ISTP_2.Context;
using Laba_ISTP_2.Models;

namespace Laba_ISTP_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrariesController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public LibrariesController(ProjectDbContext context)
        {
            _context = context;
        }

        // GET: api/Libraries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            var libraries = _context.Libraries.Include(d => d.Books).ToList();

            var librariesDtos = libraries.Select(l => new LibraryDto
            {
                Id = l.Id,
                Name = l.Name,
                Location = l.Location,
                Books = l.Books.Select(b => new BookDto
                {
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    Year = b.Year
                }).ToList()
            }).ToList();
            var message = "Success";
            Console.WriteLine(message);
            return Ok(librariesDtos);
        }

        // GET: api/Libraries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Library>> GetLibrary(int id)
        {
            var libraries = _context.Libraries.Include(d => d.Books).FirstOrDefault(d => d.Id == id);
            if (libraries == null)
            {
                return NotFound();
            }

            var librariesDto = new LibraryDto
            {
                Id = id,
                Name = libraries.Name,
                Location = libraries.Location,
                Books = libraries.Books.Select(b => new BookDto
                {
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    Year = b.Year
                }).ToList()
            };

            return Ok(librariesDto);
        }

        // PUT: api/Libraries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibrary(int id, Library library)
        {
            if (id != library.Id)
            {
                return BadRequest();
            }

            _context.Entry(library).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryExists(id))
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

        // POST: api/Libraries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Library>> CreateLibrary([FromBody] LibraryDto libraryDto)
        {
            var library = new Library
            {
                Name = libraryDto.Name,
                Location = libraryDto.Location
            };

            _context.Libraries.Add(library);
            await _context.SaveChangesAsync();


            return Ok(library);
        }

        // DELETE: api/Libraries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibrary(int id)
        {
            if (_context.Libraries == null)
            {
                return NotFound();
            }
            var library = await _context.Libraries.FindAsync(id);
            if (library == null)
            {
                return NotFound();
            }

            _context.Libraries.Remove(library);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LibraryExists(int id)
        {
            return (_context.Libraries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

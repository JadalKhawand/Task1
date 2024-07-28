using internTask1.Data;
using internTask1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace internTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BooksController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetBooks()
        {
            List<Books> books = new List<Books>();
            books = _context.Books.ToList();
            if (books == null || books.Count == 0)
            {
                return NotFound();
            }
            return Ok(books);

        }
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostBook(Books book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooks), new { id = book.BookID }, book);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updateBooks(int id, Books book)
        {
            if (id != book.BookID)
            {
                return BadRequest();
            }

            _context.Books.Update(book);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!bookExists(id))
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
        private bool bookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}

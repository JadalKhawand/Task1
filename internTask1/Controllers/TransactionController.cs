using internTask1.Data;
using internTask1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace internTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TransactionController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetTransaction()
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions = _context.Transactions.ToList();
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound();
            }
            return Ok(transactions);

        }
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest();
            }
            var memberExists = _context.Members.FirstOrDefault(x => x.MemberID == transaction.MemberID);
            if(memberExists == null)
            {
                return BadRequest("member not found");
            }

            var bookExists = _context.Books.FirstOrDefault(x => x.BookID == transaction.BookID);
            if (bookExists == null)
            {
                return BadRequest("book not found");
            }


            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionID }, transaction);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.TransactionID)
            {
                return BadRequest();
            }
            var memberExists = _context.Members.FirstOrDefault(x => x.MemberID == transaction.MemberID);
            if (memberExists == null)
            {
                return BadRequest("member not found");
            }

            var bookExists = _context.Books.FirstOrDefault(x => x.BookID == transaction.BookID);
            if (bookExists == null)
            {
                return BadRequest("book not found");
            }

            _context.Transactions.Update(transaction);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionID == id);
        }

    }
}

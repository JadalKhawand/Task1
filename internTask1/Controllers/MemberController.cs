using internTask1.Data;
using internTask1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace internTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MemberController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetMembers()
        {
            List<Members> members = new List<Members>();
            members = _context.Members.ToList();
            if (members == null || members.Count == 0)
            {
                return NotFound();
            }
            return Ok(members);

        }
        [HttpPost]
        public async Task<ActionResult<Members>> PostMember(Members member)
        {
            if (member == null)
            {
                return BadRequest();
            }

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembers), new { id = member.MemberID }, member);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updateMember(int id, Members member)
        {
            if (id != member.MemberID)
            {
                return BadRequest();
            }

            _context.Members.Update(member);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!memberExists(id))
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
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool memberExists(int id)
        {
            return _context.Members.Any(e => e.MemberID == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoworkingSpaceAPI.Models;

namespace CoworkingSpaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeskController : ControllerBase
    {
        private readonly CoworkingSpaceDbContext _context;

        public DeskController(CoworkingSpaceDbContext context)
        {
            _context = context;
        }

        // GET: api/Desk
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Desk>>> GetDesks()
        {
            return await _context.Desks.ToListAsync();
        }

        // GET: api/Desk/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Desk>> GetDesk(int id)
        {
            var desk = await _context.Desks.FindAsync(id);

            if (desk == null)
            {
                return NotFound();
            }

            return desk;
        }

        // PUT: api/Desk/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesk(int id, Desk desk)
        {
            if (id != desk.DeskId)
            {
                return BadRequest();
            }

            _context.Entry(desk).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeskExists(id))
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

        // POST: api/Desk
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Desk>> PostDesk(Desk desk)
        {
            _context.Desks.Add(desk);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDesk", new { id = desk.DeskId }, desk);
        }

        // DELETE: api/Desk/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesk(int id)
        {
            var desk = await _context.Desks.FindAsync(id);
            if (desk == null)
            {
                return NotFound();
            }

            _context.Desks.Remove(desk);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeskExists(int id)
        {
            return _context.Desks.Any(e => e.DeskId == id);
        }
    }
}

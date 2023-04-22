using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API;
using API.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassagensController : ControllerBase
    {
        private readonly APIContext _context;

        public PassagensController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Passagems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Passagem>>> GetPassagem()
        {
          if (_context.Passagem == null)
          {
              return NotFound();
          }
            return await _context.Passagem.ToListAsync();
        }

        // GET: api/Passagems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Passagem>> GetPassagem(int id)
        {
          if (_context.Passagem == null)
          {
              return NotFound();
          }
            var passagem = await _context.Passagem.FindAsync(id);

            if (passagem == null)
            {
                return NotFound();
            }

            return passagem;
        }

        // PUT: api/Passagems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPassagem(int id, Passagem passagem)
        {
            if (id != passagem.Id)
            {
                return BadRequest();
            }

            _context.Entry(passagem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassagemExists(id))
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

        // POST: api/Passagems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Passagem>> PostPassagem(Passagem passagem)
        {
          if (_context.Passagem == null)
          {
              return Problem("Entity set 'APIContext.Passagem'  is null.");
          }
            _context.Passagem.Add(passagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPassagem", new { id = passagem.Id }, passagem);
        }

        // DELETE: api/Passagems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassagem(int id)
        {
            if (_context.Passagem == null)
            {
                return NotFound();
            }
            var passagem = await _context.Passagem.FindAsync(id);
            if (passagem == null)
            {
                return NotFound();
            }

            _context.Passagem.Remove(passagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PassagemExists(int id)
        {
            return (_context.Passagem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

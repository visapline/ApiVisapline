using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPruebaVive.Context;
using ApiPruebaVive.Models;

namespace ApiPruebaVive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TercerosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TercerosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Terceros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Terceros>>> GetTerceros()
        {
            return await _context.Tercero.ToListAsync();
        }

        // GET: api/Terceros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Terceros>> GetTerceros(int id)
        {
            var terceros = await _context.Tercero.FindAsync(id);

            if (terceros == null)
            {
                return NotFound();
            }

            return terceros;
        }

        // PUT: api/Terceros/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTerceros(int id, Terceros terceros)
        {
            if (id != terceros.Idterceros)
            {
                return BadRequest();
            }

            _context.Entry(terceros).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TercerosExists(id))
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

        // POST: api/Terceros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Terceros>> PostTerceros(Terceros terceros)
        {
            _context.Tercero.Add(terceros);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTerceros", new { id = terceros.Idterceros }, terceros);
        }

        // DELETE: api/Terceros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTerceros(int id)
        {
            var terceros = await _context.Tercero.FindAsync(id);
            if (terceros == null)
            {
                return NotFound();
            }

            _context.Tercero.Remove(terceros);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TercerosExists(int id)
        {
            return _context.Tercero.Any(e => e.Idterceros == id);
        }
    }
}

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
    public class TarjetasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarjetasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tarjetas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarjeta>>> GetTarjeta()
        {
            return await _context.Tarjeta.ToListAsync();
        }

        // GET: api/Tarjetas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarjeta>> GetTarjeta(int id)
        {
            var tarjeta = await _context.Tarjeta.FindAsync(id);

            if (tarjeta == null)
            {
                return NotFound();
            }

            return tarjeta;
        }

        // PUT: api/Tarjetas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarjeta(int id, Tarjeta tarjeta)
        {
            if (id != tarjeta.IdTarjeta)
            {
                return BadRequest();
            }

            _context.Entry(tarjeta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarjetaExists(id))
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

        // POST: api/Tarjetas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tarjeta>> PostTarjeta(Tarjeta tarjeta)
        {
            _context.Tarjeta.Add(tarjeta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarjeta", new { id = tarjeta.IdTarjeta }, tarjeta);
        }

        // DELETE: api/Tarjetas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarjeta(int id)
        {
            var tarjeta = await _context.Tarjeta.FindAsync(id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            _context.Tarjeta.Remove(tarjeta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TarjetaExists(int id)
        {
            return _context.Tarjeta.Any(e => e.IdTarjeta == id);
        }
    }
}

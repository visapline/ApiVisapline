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
    public class PuertoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PuertoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Puertoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Puerto>>> GetPuerto()
        {
            return await _context.Puerto.ToListAsync();
        }

        // GET: api/Puertoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Puerto>> GetPuerto(int id)
        {
            var puerto = await _context.Puerto.FindAsync(id);

            if (puerto == null)
            {
                return NotFound();
            }

            return puerto;
        }

        // PUT: api/Puertoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPuerto(int id, Puerto puerto)
        {
            if (id != puerto.IdPuerto)
            {
                return BadRequest();
            }

            _context.Entry(puerto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuertoExists(id))
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

        // POST: api/Puertoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Puerto>> PostPuerto(Puerto puerto)
        {
            _context.Puerto.Add(puerto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPuerto", new { id = puerto.IdPuerto }, puerto);
        }

        // DELETE: api/Puertoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePuerto(int id)
        {
            var puerto = await _context.Puerto.FindAsync(id);
            if (puerto == null)
            {
                return NotFound();
            }

            _context.Puerto.Remove(puerto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PuertoExists(int id)
        {
            return _context.Puerto.Any(e => e.IdPuerto == id);
        }
    }
}

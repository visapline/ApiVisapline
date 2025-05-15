using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPruebaVive.Context;
using ApiPruebaVive.Models;
using ApiPruebaVive.Dto;

namespace ApiPruebaVive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OltsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OltsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Olts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Olt>>> GetOlt()
        {
            return await _context.Olt.ToListAsync();
        }

        // GET: api/Olts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Olt>> GetOlt(int id)
        {
            var olt = await _context.Olt.FindAsync(id);

            if (olt == null)
            {
                return NotFound();
            }

            return olt;
        }

        // PUT: api/Olts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOlt(int id, Olt olt)
        {
            if (id != olt.IdOlt)
            {
                return BadRequest();
            }

            _context.Entry(olt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OltExists(id))
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

        // POST: api/Olts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Olt>> PostOlt(Olt olt)
        {
            _context.Olt.Add(olt);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOlt", new { id = olt.IdOlt }, olt);
        }

        // DELETE: api/Olts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOlt(int id)
        {
            var olt = await _context.Olt.FindAsync(id);
            if (olt == null)
            {
                return NotFound();
            }

            _context.Olt.Remove(olt);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool OltExists(int id)
        {
            return _context.Olt.Any(e => e.IdOlt == id);
        }



        // En OltsController.cs

        [HttpPost("seleccionar-puerto")]
        public async Task<IActionResult> SeleccionarPuerto([FromBody] SeleccionarPuertoRequest request)
        {
            try
            {
                var olt = await _context.Olt
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.NombreOlt == request.TxtOlt);

                if (olt == null)
                    return NotFound("OLT no encontrada");

                // Obtengo atributos, por si los necesitas luego (pero no para IP y puerto)
                var atributos = await _context.AtributoOlt
                    .Where(a => a.IdOlt == olt.IdOlt)
                    .ToListAsync();

                var ip = olt.Ip;
                var puertoTelnet = olt.Puerto;

                if (string.IsNullOrWhiteSpace(ip) || puertoTelnet <= 0)
                    return StatusCode(500, "Faltan datos de conexión (IP o puerto)");

                // Probar conexión Telnet
                var telnet = new TelnetConnection(ip, puertoTelnet);
                var loginRespuesta = telnet.Login(olt.UsuarioOlt, olt.ContraseniaOlt, 3000);
                telnet.desconectar();

                return Ok(new
                {
                    mensaje = "Conexión Telnet exitosa",
                    respuestaLogin = loginRespuesta
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, "Error interno");
            }
        }


    }
}

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
    public class FacturasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FacturasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFactura()
        {
            return await _context.Factura
                .Include(f => f.Contrato) // <-- Esto sí carga el contrato asociado
                .Take(100) // <-- Limita la respuesta
                .ToListAsync();
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            var factura = await _context.Factura.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            return factura;
        }

        // PUT: api/Facturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(int id, Factura factura)
        {
            if (id != factura.IdFactura)
            {
                return BadRequest();
            }

            _context.Entry(factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
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

        // POST: api/Facturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(Factura factura)
        {
            _context.Factura.Add(factura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFactura", new { id = factura.IdFactura }, factura);
        }

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            var factura = await _context.Factura.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            _context.Factura.Remove(factura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacturaExists(int id)
        {
            return _context.Factura.Any(e => e.IdFactura == id);
        }

        [HttpGet("facturas")]
        public async Task<IActionResult> GetFacturas(DateTime? fechaInicio, DateTime? fechaFin, string tipo = "1", bool pagado = false)
        {
            string condicion = pagado ? "AND pg.factura_idfactura IS NOT NULL" : "";

            string fecha1 = fechaInicio?.ToString("yyyy-MM-dd") ?? "1900-01-01";
            string fecha2 = fechaFin?.ToString("yyyy-MM-dd") ?? "2100-01-01";

            string sql = $@"
    SELECT 
        factura.idfactura, 
        facturaventa, 
        CONCAT(terceros.idterceros, ABS(factura.idcont_consecutivo)) AS referencia,
        fechaemision AS FechaEmision,
        resol_factura.codigo,
        fechacorte AS corte,
        valorfac, ivafac, centrocosto.numerocentro,
        tipodoc, 
        (CASE WHEN tipodoc = 'CEDULA' THEN false ELSE true END) AS razon,
        terceros.nombre, 
        terceros.apellido, 
        identificacion,
        (CASE WHEN estrato::INTEGER < 3 THEN false ELSE true END) AS iva,
        terceros.direccion, 
        pais.codigo AS codepais, 
        departamento.codigo AS codeestado,
        municipio.codigo AS codeciudad, 
        correo,
        (SELECT telefono FROM telefono WHERE telefono.terceros_idterceros = idterceros LIMIT 1) AS telefono,
        resol_factura.prefijocontable, 
        resol_factura.paiment,
        factura_syn_siigo.idcodigo, 
        factura_syn_siigo.observacion,
        resol_factura.modalidad, 
        resol_factura.idresolucion
    FROM factura
    INNER JOIN cont_consecutivo 
        ON factura.idcont_consecutivo = cont_consecutivo.idcont_consecutivo
    INNER JOIN terceros 
        ON tercero_idtercero = idterceros
    INNER JOIN public.cargotercero ct 
        ON ct.terceros_idterceros = terceros.idterceros 
        AND ct.tipotercero_idtipotercero = {tipo}
    INNER JOIN tipodoc 
        ON idtipodoc = tipodoc_idtipodoc
    INNER JOIN resol_factura 
        ON resolucion = idresolucion
    INNER JOIN centrocosto 
        ON cont_consecutivo.idcentrocosto = centrocosto.idcentrocosto
    LEFT JOIN barrios 
        ON barrio_idbarrio = idbarrios
    LEFT JOIN municipio 
        ON municipio_idmunicipio = idmunicipio
    LEFT JOIN departamento 
        ON departamento_iddepartamento = iddepartamento
    LEFT JOIN pais 
        ON pais_idpais = idpais
    LEFT JOIN factura_syn_siigo 
        ON factura.idfactura = factura_syn_siigo.idfactura
    LEFT JOIN public.pagos pg 
        ON pg.factura_idfactura = factura.idfactura 
        AND pg.estado = 'ACTIVO'
    WHERE (fechaemision BETWEEN '{fecha1}' AND '{fecha2}') {condicion}
    ORDER BY factura.idfactura
    LIMIT 1000
    ";

            var resultado = await _context.Set<FacturaDTO>().FromSqlRaw(sql).ToListAsync();

            return Ok(resultado);
        }



    }



    // filtros por fecha en la API


}

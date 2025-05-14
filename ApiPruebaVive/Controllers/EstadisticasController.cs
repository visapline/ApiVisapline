using ApiPruebaVive.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPruebaVive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstadisticasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("contratos")]
        public async Task<ActionResult<IEnumerable<object>>> GetEstadisticasContratos()
        {
            var contratoCounts = await _context.Contrato
                .GroupBy(c => c.EstadoC)
                .Select(group => new
                {
                    Estado = group.Key,
                    Cantidad = group.Count()
                })
                .ToListAsync();

            return Ok(contratoCounts);
        }

        [HttpGet("servicios")]
        public async Task<ActionResult<IEnumerable<Object>>> GetEstadisticasServicios ()
        {
            var servicioCounts = await _context.Servicio
                .GroupBy(s => s.Estado)
                .Select(group => new
                {
                    Servicio = group.Key,
                    Cantidad = group.Count()
                })
                .ToListAsync();
            return Ok(servicioCounts);
        }

        [HttpGet("resumen")]
        public async Task<ActionResult<object>> GetResumenEstadisticas()
        {
            // Definir fechas como UTC explícitamente
            DateTime inicioMes = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime finMes = new DateTime(2025, 5, 31, 23, 59, 59, DateTimeKind.Utc);

            var ticketsAbiertos = await _context.Incidencia
                .CountAsync(i => i.Estado == "ABIERTO");

            var pqrsActivos = await _context.Ticket
                .CountAsync(t => t.EstaTicket == "ACTIVO");

            var ordenesPendientes = await _context.OrdenSalida
                .CountAsync(o => o.Estado == false);

            var facturasMes = await _context.Factura
                .CountAsync(f => f.FechaEmision >= inicioMes && f.FechaEmision <= finMes);

            var contratosMes = await _context.Contrato
                .CountAsync(c => c.FechaContrato >= inicioMes && c.FechaContrato <= finMes);

            var porInstalar = await _context.Contrato
                .CountAsync(c => c.EstadoC == "VENDIENDO");

            var resultado = new
            {
                Tickets = ticketsAbiertos,
                PQRSDF = pqrsActivos,
                Ordenes = ordenesPendientes,
                Facturas = facturasMes,
                Registrados = contratosMes,
                PorInstalar = porInstalar
            };

            return Ok(resultado);
        }

    }
}

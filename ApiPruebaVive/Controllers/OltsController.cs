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
using Npgsql;
using ApiPruebaVive.Services.Parsers;
using ApiPruebaVive.Services;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;

namespace ApiPruebaVive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OltsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TelnetConnectionManager _telnetManager;

        public OltsController(AppDbContext context, TelnetConnectionManager telnetManager)
        {
            _context = context;
            _telnetManager = telnetManager;
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

                var ip = olt.Ip;
                var puertoTelnet = olt.Puerto;

                if (string.IsNullOrWhiteSpace(ip) || puertoTelnet <= 0)
                    return StatusCode(500, "Faltan datos de conexión (IP o puerto)");

                TelnetConnection telnet;
                try
                {
                    telnet = _telnetManager.GetConnection(olt.NombreOlt);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"No se pudo obtener la conexión Telnet: {ex.Message}");
                }

                telnet.WriteLine("terminal length 0");
                await Task.Delay(200);

                telnet.WriteLine($"show gpon onu state gpon_olt-1/{request.Tarjeta}/{request.Puerto}");
                string resultadoOnus = telnet.Read();

                telnet.WriteLine($"show pon power onu-rx gpon_olt-1/{request.Tarjeta}/{request.Puerto}");
                string resultadoRx = telnet.Read();

                telnet.WriteLine($"show pon power onu-tx gpon_olt-1/{request.Tarjeta}/{request.Puerto}");
                string resultadoTx = telnet.Read();

               

                var parser = new Zte600Parser();

                var dispositivos = parser.ObtenerDispositivos(resultadoOnus, olt.NombreOlt);
                dispositivos = parser.ObtenerPotenciasRXZte(resultadoRx, olt.NombreOlt, dispositivos);
                dispositivos = parser.ObtenerPotenciasTXZte(resultadoTx, olt.NombreOlt, dispositivos);

                // Consultar servicios desde la base de datos para la OLT/tarjeta/puerto
                var servicios = await ConsultarServiciosAsync(olt.IdOlt, request.Tarjeta, int.Parse(request.Puerto));

                // Cruzar info servicios con dispositivos
                foreach (var d in dispositivos)
                {
                    string onuIndex = d.OnuIndex;
                    string onuNumberStr = onuIndex.Split(':').Last();
                    int onuNumber = int.Parse(onuNumberStr);
                    // Ajusta el criterio de búsqueda si es necesario (aquí usamos referencia de tarjeta y puerto numérico)
                    var servicio = servicios.FirstOrDefault(s =>
                        s.ReferenciaTarjeta == d.Tarjeta &&
                        s.NumeroPuerto == int.Parse(d.Puerto) &&
                        s.IndicePuerto == onuNumber
                        );

                    if (servicio != null)
                    {
                        d.UserName = servicio.Usuario ?? servicio.UsuarioServicio ?? "";
                        d.EstadoDb = servicio.EstadoServicio ?? "";
                        d.EstadocDb = servicio.EstadoContrato ?? "";
                        d.SerialDb = servicio.Serial ?? "";
                        d.IdentificacionDb = servicio.Identificacion ?? "";

                        // Estados visuales (solo datos, sin HTML)
                        d.EstadoRXVisual = (double.TryParse(d.RX, out double rxVal) && rxVal > -28) ? "ok" : "warning";
                        d.EstadoPhaseStateVisual = d.PhaseState?.ToLower() == "working" ? "ok" : "offline";
                    }
                }

                return Ok(new
                {
                    mensaje = "Conexión Telnet exitosa",
                    dispositivos = dispositivos
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, "Error interno");
            }
        }





        private async Task<List<ServicioDetalleDto>> ConsultarServiciosAsync(int idOlt, string tarjeta, int puerto)
        {
            var sql = @"
            SELECT 
    t.codigo AS Codigo,
    t.identificacion AS Identificacion,
    CONCAT(t.nombre, ' ', t.apellido) AS Usuario,
    t.tipoterceros AS TipoTercero,
    c.idcont_consecutivo AS IdContrato,
    ss.idservicios AS IdServicio,
    cc.estadoc AS EstadoContrato,
    ss.estado AS EstadoServicio,
    ss.usser AS UsuarioServicio,
    ss.inventario_idinventario AS InventarioId,
    ss.routerboard_idrouterboard AS RouterboardId,
    ss.direccionip AS DireccionIp,
    ss.""IndicePuerto"" AS IndicePuerto,
    ss.""Puerto_idPuerto"" AS PuertoId,
    iv.idinventario AS InventarioId2,
    pp.barrios_idbarrios AS BarrioId,
    bb.barrios AS Barrio,
    pp.direcion AS Direccion,
    pp.tipo AS TipoDireccion,
    iv.serial AS Serial,
    iv.estado AS EstadoInventario,
    puerto.numero AS NumeroPuerto,
    tarjeta.referencia AS ReferenciaTarjeta
FROM view_terceros(null, null) t 
INNER JOIN cont_consecutivo c ON c.tercero_idtercero = t.codigo 
INNER JOIN contrato cc ON cc.codigo = c.idcont_consecutivo 
INNER JOIN servicios ss ON ss.contrato_idcontrato = cc.idcontrato 
LEFT JOIN inventario iv ON iv.idinventario = ss.inventario_idinventario 
INNER JOIN puntos pp ON pp.idpuntos = ss.puntos_idpuntos 
INNER JOIN barrios bb ON pp.barrios_idbarrios = bb.idbarrios 
INNER JOIN puerto ON ss.""Puerto_idPuerto"" = puerto.idpuerto 
INNER JOIN tarjeta ON puerto.tarjeta_idtarjeta = tarjeta.idtarjeta 
WHERE tarjeta.olt_idolt = @idOlt AND tarjeta.referencia = @tarjeta AND puerto.numero = @puerto

";

            var servicios = await _context.ServiciosDto
                .FromSqlRaw(sql,
                    new Npgsql.NpgsqlParameter("idOlt", idOlt),
                    new Npgsql.NpgsqlParameter("tarjeta", tarjeta),
                    new Npgsql.NpgsqlParameter("puerto", puerto)
                )
                .ToListAsync();

            return servicios;
        }


        //[HttpGet("listar-tarjetas")]
        //public IActionResult ListarTarjetas()
        //{
        //    try
        //    {
        //        string nombreOlt = "ZTEC600"; // OLT fija que ya está conectada

        //        TelnetConnection telnet;
        //        try
        //        {
        //            telnet = _telnetManager.GetConnection(nombreOlt); // ✅ conexión activa o reconectada
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, $"No se pudo obtener la conexión Telnet: {ex.Message}");
        //        }

        //        telnet.WriteLine("terminal length 0");
        //        Thread.Sleep(200); // No se puede usar await/async aquí, el telnet actual es síncrono

        //        telnet.WriteLine("show card");
        //        string resultado = telnet.Read();

        //        var tarjetas = ParseTarjetasCon16Puertos(resultado);

        //        var tarjetasDto = tarjetas.Select(t => new
        //        {
        //            tarjeta = t.tarjeta,
        //            tipo = t.tipo,
        //            estado = t.estado
        //        });

        //        return Ok(new
        //        {
        //            mensaje = "Listado de tarjetas con 16 puertos",
        //            tarjetas = tarjetasDto
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"ERROR: {ex.Message}");
        //        Console.WriteLine(ex.StackTrace);
        //        return StatusCode(500, "Error interno");
        //    }
        //}

        //// DTO
        //public class ListarTarjetasRequest
        //{
        //    public string TxtOlt { get; set; }
        //}

        //// Método auxiliar
        //private List<(string tarjeta, int puertos, string tipo, string estado)> ParseTarjetasCon16Puertos(string salidaTelnet)
        //{
        //    var resultado = new List<(string tarjeta, int puertos, string tipo, string estado)>();

        //    var lineas = salidaTelnet.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        //    bool inicioTabla = false;

        //    foreach (var linea in lineas)
        //    {
        //        if (linea.StartsWith("----"))
        //        {
        //            inicioTabla = true;
        //            continue;
        //        }

        //        if (!inicioTabla) continue;

        //        var partes = linea.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //        if (partes.Length < 7) continue;

        //        var slot = partes[1];
        //        var cfgType = partes[2];
        //        var portCountStr = partes[4];
        //        var estado = partes[6];

        //        if (!int.TryParse(portCountStr, out int portCount)) continue;

        //        if (portCount == 16)
        //        {
        //            resultado.Add((tarjeta: slot, puertos: portCount, tipo: cfgType, estado: estado));
        //        }
        //    }

        //    return resultado;
        //}



        //[HttpGet("tarjetas/puertos")]
        //public IActionResult ObtenerPuertosDeTodasLasTarjetas()
        //{
        //    var tarjetas = new[] { "2", "3", "4", "5", "6",/* "7", "8", "9", "12", "13", "14", "15", "16", "17"*/ };
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    string nombreOlt = "ZTEC600";
        //    TelnetConnection telnet;

        //    try
        //    {
        //        telnet = _telnetManager.GetConnection(nombreOlt);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"No se pudo obtener la conexión Telnet: {ex.Message}");
        //    }

        //    telnet.WriteLine("terminal length 0");
        //    telnet.ReadUntilPrompt(); // limpiar buffer inicial

        //    var resultadoTarjetas = new List<object>();

        //    foreach (var numero in tarjetas)
        //    {
        //        var comandos = new StringBuilder();
        //        for (int puerto = 1; puerto <= 16; puerto++)
        //        {
        //            comandos.AppendLine($"show interface gpon_olt-1/{numero}/{puerto}");
        //        }

        //        telnet.WriteLine(comandos.ToString());
        //        string outputTotal = telnet.ReadUntilPrompt("ZXAN#", 8000); // ajusta timeout si hace falta

        //        var puertos = new List<object>();

        //        for (int puerto = 1; puerto <= 16; puerto++)
        //        {
        //            string siguiente = puerto < 16
        //                ? $"gpon_olt-1/{numero}/{puerto + 1}"
        //                : "ZXAN#";

        //            string patron = $"gpon_olt-1/{numero}/{puerto}.*?(?={siguiente})";
        //            var match = Regex.Match(outputTotal, patron, RegexOptions.Singleline);
        //            string resultado = match.Success ? match.Value : "";

        //            string estado = ParseEstadoPuerto(resultado);

        //            puertos.Add(new
        //            {
        //                puerto = puerto.ToString(),
        //                estado,
        //                admin = "",
        //                link = "",
        //                color = GetColorForEstado(estado)
        //            });
        //        }

        //        resultadoTarjetas.Add(new
        //        {
        //            tarjeta = numero,
        //            puertos
        //        });
        //    }

        //    stopwatch.Stop();

        //    return Ok(new
        //    {
        //        mensaje = "Resultado completo de todas las tarjetas",
        //        tiempoTotalMs = stopwatch.ElapsedMilliseconds,
        //        tarjetas = resultadoTarjetas
        //    });
        //}



        //private string GetColorForEstado(string estado)
        //{
        //    if (estado.Contains("Link Up")) return "green";
        //    if (estado.Contains("Link Down") || estado == "Inactivo") return "red";
        //    return "gray"; // desconocido o neutro
        //}


        //private string ParseEstadoPuerto(string salida)
        //{
        //    // Ejemplo de línea esperada:
        //    // "gpon_olt-1/2/1 is activate,line protocol is up."

        //    var lines = salida.Split('\n');
        //    var lineaEstado = lines.FirstOrDefault(l => l.Contains("is activate") || l.Contains("is deactive"));

        //    if (lineaEstado == null)
        //        return "Desconocido";

        //    bool activo = lineaEstado.Contains("activate");
        //    bool linkUp = lineaEstado.Contains("line protocol is up");

        //    if (activo && linkUp) return "Activo - Link Up";
        //    if (activo && !linkUp) return "Activo - Link Down";
        //    if (!activo) return "Inactivo";

        //    return "Desconocido";
        //}



    }
}

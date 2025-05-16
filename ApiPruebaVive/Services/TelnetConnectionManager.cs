using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ApiPruebaVive.Context;
using ApiPruebaVive.Models;
using Microsoft.EntityFrameworkCore;


namespace ApiPruebaVive.Services
{
    public class TelnetConnectionManager
    {
        private readonly AppDbContext _context; // o tu contexto

        private static ConcurrentDictionary<string, TelnetConnection> _connections = new();

        public TelnetConnectionManager(AppDbContext context)
        {
            _context = context;
        }

        // Inicializar todas las conexiones leyendo de DB
        public async Task InitializeConnectionsAsync()
        {
            var olts = await _context.Olt.AsNoTracking()
                           .Where(o => o.NombreOlt == "ZTEC600")
                           .ToListAsync();

            foreach (var olt in olts)
            {
                try
                {
                    var telnet = new TelnetConnection(olt.Ip, olt.Puerto);
                    var loginResponse = telnet.Login(olt.UsuarioOlt, olt.ContraseniaOlt, 3000);

                    if (loginResponse.Contains("ZXAN#"))
                    {
                        _connections[olt.NombreOlt] = telnet;
                        Console.WriteLine($"✅ Conexión establecida correctamente con la OLT: {olt.NombreOlt} ({olt.Ip})");
                    }
                    else
                    {
                        telnet.desconectar();
                        Console.WriteLine($"⚠️ No se pudo autenticar con la OLT: {olt.NombreOlt} ({olt.Ip})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al conectar a la OLT {olt.NombreOlt} ({olt.Ip}): {ex.Message}");
                }
            }
        }


        // Obtener conexión ya abierta por nombre
        public TelnetConnection GetConnection(string nombreOlt)
        {
            if (_connections.TryGetValue(nombreOlt, out var conn))
            {
                if (conn.IsConnected)
                    return conn;

                // Si no está conectado, intenta reconectar automáticamente (bloqueante)
                Console.WriteLine($"⚠️ Conexión perdida con {nombreOlt}. Intentando reconectar...");
                var task = ReconnectAsync(nombreOlt);
                task.Wait(); // bloqueante

                if (task.Result)
                {
                    return _connections[nombreOlt];
                }
            }

            throw new Exception($"❌ No se pudo obtener conexión activa con la OLT {nombreOlt}.");
        }

        // Cerrar todas las conexiones (cuando apaga API)
        public void DisposeAll()
        {
            foreach (var conn in _connections.Values)
            {
                try { conn.desconectar(); } catch { }
            }
            _connections.Clear();
        }



        public async Task<bool> ReconnectAsync(string nombreOlt)
        {
            var olt = await _context.Olt
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.NombreOlt == nombreOlt);

            if (olt == null)
                return false;

            try
            {
                var telnet = new TelnetConnection(olt.Ip, olt.Puerto);
                var loginResponse = telnet.Login(olt.UsuarioOlt, olt.ContraseniaOlt, 3000);

                if (loginResponse.Contains("ZXAN#"))
                {
                    _connections[olt.NombreOlt] = telnet;
                    Console.WriteLine($"🔄 Reconexión exitosa con la OLT: {olt.NombreOlt}");
                    return true;
                }

                telnet.desconectar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al reconectar con la OLT {nombreOlt}: {ex.Message}");
            }

            return false;
        }

    }
}

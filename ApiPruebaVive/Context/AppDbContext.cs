using Microsoft.EntityFrameworkCore;
using ApiPruebaVive.Models;
using ApiPruebaVive.Dto; // Asegúrate de incluir este using

namespace ApiPruebaVive.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet para la tabla 'Terceros' en la base de datos
        public DbSet<Terceros> Tercero { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Contrato> Contrato { get; set; }

        public DbSet<Olt> Olt { get; set; } // DbSet para la tabla 'olt' en la base de datos
        public DbSet<Factura> Factura { get; set; } // DbSet para la tabla 'factura' en la base de datos
        public DbSet<FacturaDTO> FacturasDto { get; set; } // Sin [Key], pero debe estar mapeado
        public DbSet<Tarjeta> Tarjeta { get; set; } // DbSet para la tabla 'tarjeta' en la base de datos
        public DbSet<Puerto> Puerto { get; set; } // DbSet para la tabla 'puerto' en la base de datos
        public DbSet<OrdenSalida> OrdenSalida { get; set; } // DbSet para la tabla 'orden_salida' en la base de datos
        public DbSet<Ticket> Ticket { get; set; } // DbSet para la tabla 'ticket' en la base de datos
        public DbSet<Incidencias> Incidencia { get; set; } // DbSet para la tabla 'incidencias' en la base de datos
        public DbSet<Servicios> Servicio { get; set; } // DbSet para la tabla 'servicios' en la base de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FacturaDTO>().HasNoKey(); // Importante: no tiene clave
        }
    }
}

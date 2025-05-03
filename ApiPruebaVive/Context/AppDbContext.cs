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
        public DbSet<Factura> Factura { get; set; }

        public DbSet<FacturaDTO> FacturasDto { get; set; } // Sin [Key], pero debe estar mapeado

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FacturaDTO>().HasNoKey(); // Importante: no tiene clave
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Models
{
    [Table("incidensias")]
    public class Incidencias
    {
        [Key]
        [Column("idincidensias")]
        public int IdIncidencias { get; set; }

        [Column("fechainicio")]
        public DateTime? FechaInicio { get; set; }

        [Column("fechafin")]
        public DateTime? FechaFin { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("costo")]
        public int? Costo { get; set; }

        [Column("detalle")]
        public string? Detalle { get; set; }

        [Column("terceros_idterceros_reg")]
        public int TercerosIdTercerosReg { get; set; }

        [Column("servicios_idservicios")]
        public int ServiciosIdServicios { get; set; }

        [Column("obervacion")]
        public string? Obervacion { get; set; }

        [Column("descuento")]
        public bool Descuento { get; set; }

        [Column("tipoincidecia_idtipoincidencia")]
        public int TipoIncideciaIdTipoIncidencia { get; set; }

        [Column("pqrs_idpqrs")]
        public int? PqrsIdPqrs { get; set; }

        [Column("tercero_idterceroclosed")]
        public int? TerceroIdTerceroClosed { get; set; }

        [Column("georeferenciasoporte")]
        public string? GeoReferenciaSoporte { get; set; }

        [Column("soportedoc")]
        public decimal? SoporteDoc { get; set; }

        [Column("tercero_asigna")]
        public int? TerceroAsigna { get; set; }

        [Column("fecha_asigna")]
        public DateTimeOffset? FechaAsigna { get; set; }

        [Column("tercero_validacierre")]
        public int? TerceroValidaCierre { get; set; }

        [Column("estadohabil")]
        public bool? EstadoHabil { get; set; }

        [Column("costo_incidencia")]
        public int? CostoIncidencia { get; set; }

        [Column("tercero_validahabil")]
        public int? TerceroValidaHabil { get; set; }

        [Column("fecha_validahabil")]
        public DateTimeOffset? FechaValidaHabil { get; set; }
    }
}

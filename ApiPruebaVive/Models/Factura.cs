using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Models
{
    [Table("factura")]
    public class Factura
    {
        [Key]
        [Column("idfactura")]
        public int IdFactura { get; set; }

        [Column("fechaemision")]
        public DateTime? FechaEmision { get; set; }

        [Column("fechavencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [Column("fechacorte")]
        public DateTime? FechaCorte { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("cuotas")]
        public int? Cuotas { get; set; }

        [Column("referenciapago")]
        public int? ReferenciaPago { get; set; }

        [Column("facturaventa")]
        public int FacturaVenta { get; set; }

        // Clave foránea
        [Column("contrato_idcontrato")]
        public int? ContratoIdContrato { get; set; }

        // Propiedad de navegación
        [ForeignKey("ContratoIdContrato")]
        public virtual Contrato Contrato { get; set; }

        [Column("dirfactura")]
        public string? DirFactura { get; set; }

        [Column("valorfac")]
        public double? ValorFac { get; set; }

        [Column("saldofac")]
        public double? SaldoFac { get; set; }

        [Column("ivafac")]
        public double? IvaFac { get; set; }

        [Column("totalfac")]
        public double? TotalFac { get; set; }

        [Column("resolucion")]
        public int Resolucion { get; set; }

        [Column("terceros_idterceros")]
        public int TercerosIdTerceros { get; set; }

        [Column("idcont_consecutivo")]
        public int? IdContConsecutivo { get; set; }

        [Column("periodoinicio")]
        public DateTime? PeriodoInicio { get; set; }

        [Column("periodofin")]
        public DateTime? PeriodoFin { get; set; }

        [Column("fechacreada")]
        public DateTime? FechaCreada { get; set; }

        [Column("idcentrocosto")]
        public int? IdCentroCosto { get; set; }
    }
}

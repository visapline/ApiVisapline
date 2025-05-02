using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiPruebaVive.Models
{
    [Table("contrato")]
    public class Contrato
    {
        [Key]
        [Column("idcontrato")]
        public int IdContrato { get; set; }

        [Column("terceros_idterceros")]
        public int TercerosIdTercerosCont { get; set; }

        [Column("fechacontrato")]
        public DateTime FechaContrato { get; set; }

        [Column("fechaactivacion")]
        public DateTime? FechaActivacion { get; set; }

        [Column("fechafacturacion")]
        public DateTime? FechaFacturacion { get; set; }

        [Column("estadoc")]
        public string EstadoC { get; set; } = string.Empty;

        [Column("tipocontrato_idtipocontrato")]
        public int TipoContratoIdTipoContrato { get; set; }

        [Column("plan_idplan")]
        public int PlanIdPlan { get; set; }

        [Column("iva")]
        public double? Iva { get; set; }

        [Column("direnviofactura")]
        public string? DirEnvioFactura { get; set; }

        [Column("enviofactura")]
        public string? EnvioFactura { get; set; }

        [Column("facturaunica")]
        public string? FacturaUnica { get; set; }

        [Column("personal_idpersonal")]
        public int PersonalIdPersonalReg { get; set; }

        [Column("sucursal_idsucursal")]
        public int? SucursalIdSucursal { get; set; }

        [Column("codigo")]
        public int? Codigo { get; set; }

        [Column("observacion")]
        public string? Observacion { get; set; }

        [Column("barrio_fac_idbarrio")]
        public int? BarrioFacIdBarrio { get; set; }

        [Column("fechafinalizacion")]
        public DateTime? FechaFinalizacion { get; set; }

        [Column("descuento")]
        public double Descuento { get; set; }

        [Column("wifi")]
        public bool? Wifi { get; set; }

      
    }
}

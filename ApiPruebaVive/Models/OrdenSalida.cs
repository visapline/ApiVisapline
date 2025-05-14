using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Models
{
    [Table("ordensalida")]
    public class OrdenSalida
        
    {
        [Key]
        [Column("idordensalidacabe")]
        public int IdOrdenSalidaCabe { get; set; } // NOT NULL

        [Column("codigo")]
        public string Codigo { get; set; } // NOT NULL

        [Column("detalle")]
        public string Detalle { get; set; } // NOT NULL

        [Column("observacion")]
        public string Observacion { get; set; } // NOT NULL

        [Column("terceros_idterceros_reg")]
        public int TercerosIdTercerosReg { get; set; } // NOT NULL

        [Column("personal_idpersonal_ati")]
        public int PersonalIdPersonalAti { get; set; } // NOT NULL

        [Column("servicios_idservicios")]
        public int ServiciosIdServicios { get; set; } // NOT NULL

        [Column("tipoorden")]
        public string TipoOrden { get; set; } // NOT NULL

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } // NOT NULL

        [Column("estado")]
        public bool Estado { get; set; } // NOT NULL

        [Column("fecha_finalizar")]
        public DateTime? FechaFinalizar { get; set; } // NULLABLE

        [Column("fechains_aprox")]
        public DateTime? FechaInsAprox { get; set; } // NULLABLE

        [Column("soportedoc")]
        public decimal? SoporteDoc { get; set; } // NULLABLE

        [Column("georeferenciasoporte")]
        public string GeoreferenciaSoporte { get; set; } // NOT NULL
    }

}

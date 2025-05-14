using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Models
{
    [Table("servicios")]
    public class Servicios
    {
        [Key]
        [Column("idservicios")]
        public int IdServicios { get; set; }

        [Column("fechainicio")]
        public DateTime? FechaInicio { get; set; }

        [Column("cantidadmegas")]
        public int? CantidadMegas { get; set; }

        [Column("contrato_idcontrato")]
        public int? ContratoIdContrato { get; set; }

        [Column("estrato")]
        public string? Estrato { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("referencias")]
        public string? Referencias { get; set; }

        [Column("puntos_idpuntos")]
        public int? PuntosIdPuntos { get; set; }

        [Column("inventario_idinventario")]
        public int? InventarioIdInventario { get; set; }

        [Column("direccionip")]
        public string? DireccionIp { get; set; }

        [Column("usser")]
        public string? Usser { get; set; }

        [Column("routerboard_idrouterboard")]
        public int? RouterboardIdRouterboard { get; set; }

        [Column("Puerto_idPuerto")]
        public int? PuertoIdPuerto { get; set; }

        [Column("IndicePuerto")]
        public int? IndicePuerto { get; set; }

        [Column("radioinventario")]
        public int? RadioInventario { get; set; }

        [Column("idconsecutivo")]
        public int IdConsecutivo { get; set; }

        [Column("idservadicional")]
        public int? IdServAdicional { get; set; }

        [Column("tipoconexion")]
        public string? TipoConexion { get; set; }
    }
}

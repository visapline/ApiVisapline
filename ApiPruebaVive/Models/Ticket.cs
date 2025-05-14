using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPruebaVive.Models
{
    [Table("ticket")]
    public class Ticket
    {
        [Key]
        [Column("idticket")]
        public int IdTicket { get; set; }

        [Column("fechacreacionticket")]
        public DateTime? FechaCreacionTicket { get; set; }

        [Column("empaticket")]
        public string? EmpaTicket { get; set; }

        [Column("estaticket")]
        public string? EstaTicket { get; set; }

        [Column("tipologiafk")]
        public int? TipologiaFk { get; set; }

        [Column("serviciosfk")]
        public int ServiciosFk { get; set; }

        [Column("medioatencionfk")]
        public int MedioAtencionFk { get; set; }

        [Column("emisorticketfk")]
        public int EmisorTicketFk { get; set; }

        [Column("areasfk")]
        public int? AreasFk { get; set; }

        [Column("descticket")]
        public string? DescTicket { get; set; }

        [Column("tipoticket")]
        public int? TipoTicket { get; set; }

        [Column("tiporeclamo")]
        public int? TipoReclamo { get; set; }

        [Column("servicios_idservicios")]
        public int? ServiciosIdServicios { get; set; }

        [Column("pqrsdf_idpqrsdftras")]
        public int? PqrsdfIdPqrsdfTras { get; set; }

        [Column("soportedocume")]
        public string? SoporteDocume { get; set; }

        [Column("fechaasigna")]
        public DateTimeOffset? FechaAsigna { get; set; }

        [Column("terceroasignado")]
        public int? TerceroAsignado { get; set; }

        [Column("fechacierre")]
        public DateTimeOffset? FechaCierre { get; set; }

        [Column("terceroasigna")]
        public int? TerceroAsigna { get; set; }

        [Column("tercerocierra")]
        public int? TerceroCierra { get; set; }
    }
}

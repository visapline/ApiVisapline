using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Models
{
    [Table("tarjeta")]
    public class Tarjeta
    {
        [Key]
        [Column("idtarjeta")]
        public int IdTarjeta { get; set; }
        [Column("referencia")]
        public string Referencia { get; set; }
        [Column("max_puerto")]
        public int MaxPuerto { get; set; }
        [Column("olt_idolt")]
        public int OltIdolt { get; set; }
        [Column("status")]
        public string Status { get; set; }
    }
}

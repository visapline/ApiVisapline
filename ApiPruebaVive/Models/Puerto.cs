using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Models
{
    [Table("puerto")]
    public class Puerto
    {
        [Key]
        [Column("idpuerto")]
        public int IdPuerto { get; set; }           // idpuerto
        [Column("numero")]
        public int Numero { get; set; }             // numero
        [Column("tarjeta_idtarjeta")]
        public int TarjetaId { get; set; }          // tarjeta_idtarjeta
        [Column("descripcion")]
        public string? Descripcion { get; set; }    // descripcion (nullable)
    }
}

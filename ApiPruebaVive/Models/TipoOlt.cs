using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPruebaVive.Models
{
    [Table("tipoolt")]
    public class TipoOlt
    {
        [Key]
        [Column("idtipoolt")]
        public int IdTipoOlt { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }
    }
}

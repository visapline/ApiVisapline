using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPruebaVive.Models
{
    [Table("atributosolt")]
    public class AtributoOlt
    {
        [Key]
        [Column("idatrib")]
        public int IdAtributo { get; set; }

        [Column("valor")]
        public string Valor { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("idolt")]
        public int IdOlt { get; set; }

      
    }
}

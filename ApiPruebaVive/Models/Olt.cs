using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Models
{
    [Table("olt")]
    public class Olt
    {
        [Key]

        [Column("idolt")]
        public int IdOlt { get; set; }
        [Column("ip")]
        public string Ip { get; set; }
        [Column("puerto")]
        public int Puerto { get; set; }
        [Column("nodo_idnodo")]
        public int NodoIdNodo { get; set; }
        [Column("idinventario_inventario")]
        public int? IdInventario { get; set; }
        [Column("usuarioolt")]
        public string UsuarioOlt { get; set; }
        [Column("contraseniaolt")]
        public string ContraseniaOlt { get; set; }
        [Column("nombreolt")]
        public string NombreOlt { get; set; }
        [Column("tipoolt")]
        public int TipoOlt { get; set; }
        [Column("centrocosto_idcentrocosto")]
        public int CentroCostoId { get; set; }
    }
}

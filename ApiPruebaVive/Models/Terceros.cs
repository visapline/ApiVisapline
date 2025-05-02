using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Models
{
    [Table("terceros")]
    public class Terceros
    {
        [Key]

        [Column("idterceros")]  // Especifica el nombre de la columna en la base de datos
        public int Idterceros { get; set; }

        [Column("estrato")]
        public string? Estrato { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("tiporesidencia_idtiporesidencia")]
        public int? TiporesidenciaIdtiporesidencia { get; set; }

        [Column("tipofactura_idtipofactura")]
        public int? TipofacturaIdtipofactura { get; set; }

        [Column("identificacion")]
        public string? Identificacion { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }
                     
        [Column("apellido")]
        public string? Apellido { get; set; }
                     
        [Column("correo")]
        public string? Correo { get; set; }
                     
        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("barrio_idbarrio")]
        public int? BarrioIdbarrio { get; set; }

        [Column("fechexp")]
        public DateTime? Fechexp { get; set; }

        [Column("tipodoc_idtipodoc")]
        public int? TipodocIdtipodoc { get; set; }

        [Column("rh")]
        public string? Rh { get; set; }

        // Propiedades que no aparecen en la tabla (deberías eliminarlas o confirmar si existen)
        // public string TipoTerceroId { get; set; }  // No existe en la tabla
        // public string FechaNacimiento { get; set; }  // No existe en la tabla (hay fechexp)
        // public string UsuarioId { get; set; }  // No existe en la tabla
        // public string Telefono { get; set; }  // No existe en la tabla
    }
}

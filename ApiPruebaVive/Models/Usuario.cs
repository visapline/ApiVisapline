using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("idusuario")]
        public int Idusuario { get; set; }

        [Column("usuario")]
        [Required]
        [StringLength(45)]
        public string User { get; set; }

        [Column("contrasena")]
        [Required]
        [StringLength(45)]
        public string Contrasena { get; set; }

        [Column("terceros_idterceros")]
        public int Idtercero { get; set; }

        [ForeignKey("Idtercero")] // Relación con Terceros
        public virtual Terceros Tercero { get; set; } // Propiedad de navegación

        [Column("image")]
        [StringLength(255)]
        public string? Image { get; set; }

        [Column("huella")]
        [StringLength(255)]
        public string? Huella { get; set; }

       
    }
}
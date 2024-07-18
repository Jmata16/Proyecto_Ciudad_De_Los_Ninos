using API_Ciudad_De_Los_Ninos.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Ciudad_De_Los_Ninos.Models
{
    public class Tickete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo usuario es requerido")]
        [Column("id_usuario")]
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "El campo Higiene personal es requerido para el producto")]
        [Column("id_inventario_higiene_personal")]
        public int id_inventario_higiene_personal { get; set; }

        [Required(ErrorMessage = "El campo Tickete es requerido")]
        public int tickete { get; set; }

        [ForeignKey("id_inventario_higiene_personal")]
        public Inventario_Higiene_Personal? inventario_Higiene_Personal { get; set; }

        [ForeignKey("id_usuario")]
        public User? Usuario { get; set; }
    }
}

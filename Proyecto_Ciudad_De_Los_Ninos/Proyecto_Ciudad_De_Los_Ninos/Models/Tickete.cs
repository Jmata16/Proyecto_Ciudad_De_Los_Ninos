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

        [Required(ErrorMessage = "El campo joven es requerido")]
        public int id_joven { get; set; }

        [Required(ErrorMessage = "El campo Higiene personal es requerido para el producto")]
        public int id_inventario_higiene_personal { get; set; }

        [Required(ErrorMessage = "El campo Tickete es requerido")]
        public int tickete { get; set; }

        public Inventario_Higiene_Personal? inventario_Higiene_Personal { get; set; }

        public Jovenes? Joven { get; set; }
      
    
    }
}

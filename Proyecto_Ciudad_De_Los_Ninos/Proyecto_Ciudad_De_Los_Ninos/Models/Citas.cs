using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Citas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Usuario es requerido")]
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "El campo Joven es requerido")]
        public int id_joven { get; set; }

        [Required(ErrorMessage = "El campo Fecha es requerido")]
        public DateTime fecha { get; set; }


        [Required(ErrorMessage = "El campo Detalles es requerido")]
        public string detalles { get; set; }

        public string Estado { get; set; } = "Activo";

        public User? Usuario { get; set; }
        public Jovenes? Joven { get; set; }
    }

}

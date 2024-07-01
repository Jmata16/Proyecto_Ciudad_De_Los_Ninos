using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Pruebas_Dopaje
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "Este campo no puede ser un número negativo.")]
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "Este campo no puede ser un número negativo.")]
        public int id_joven { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        public DateTime fecha_hora { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [StringLength(255, ErrorMessage = "Este campo es requerido.")]
        public string lugar { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [StringLength(8)]
        [RegularExpression("^(Positivo|Negativo)$", ErrorMessage = "Este campo es requerido.")]
        public string resultado { get; set; }

        [StringLength(500, ErrorMessage = "Este campo es requerido.")]
        public string observaciones { get; set; }

        [ForeignKey("id_usuario")]
        public User? Usuario { get; set; }

        [ForeignKey("id_joven")]
        public Jovenes? Joven { get; set; }
    }
}

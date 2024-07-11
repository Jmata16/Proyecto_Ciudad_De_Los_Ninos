using API_Ciudad_De_Los_Ninos.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Ciudad_De_Los_Ninos.Models
{
    public class Vacaciones
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int id_usuario { get; set; }

        [StringLength(255, ErrorMessage = "El estado no puede exceder los 255 caracteres.")]
        public string estado { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime fechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de finalización es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime fechaFinaliza { get; set; }

        public string justificacion { get; set; }

        public virtual User? User { get; set; }
    }
}

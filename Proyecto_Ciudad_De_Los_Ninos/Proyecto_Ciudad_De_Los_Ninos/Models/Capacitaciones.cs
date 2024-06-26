using API_Ciudad_De_Los_Ninos.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Ciudad_De_Los_Ninos.Models
{
    public class Capacitaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        [ForeignKey("User")]
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "El nombre de la capacitación es obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre de la capacitación no puede exceder los 255 caracteres.")]
        public string nombre_capacitacion { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha no válido.")]
        [Range(typeof(DateTime), "01/01/1900", "01/01/2100", ErrorMessage = "La fecha debe estar entre el 01/01/1900 y el 01/01/2100.")]
        public DateTime fecha { get; set; }

        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        public string descripcion { get; set; }

        public virtual User? User { get; set; }
    }
}

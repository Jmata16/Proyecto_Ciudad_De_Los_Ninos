using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Jovenes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Cédula es obligatorio.")]
        public int cedula { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El campo Edad es obligatorio.")]
        public int edad { get; set; }

        [Required(ErrorMessage = "El campo Dirección es obligatorio.")]
        public string direccion { get; set; }

        [Required(ErrorMessage = "El campo localización es obligatorio para saber donde se hospeda el joven.")]
        public string Localizacion { get; set; }

        [Required(ErrorMessage = "El campo Teléfono de Contacto es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El formato del teléfono debe ser de 8 dígitos.")]
        public string telefono_contacto { get; set; }

        public ICollection<Expedientes>? Expedientes { get; set; }
        public ICollection<Reportes_Medicos>? ReportesMedicos { get; set; }
        public ICollection<Pruebas_Dopaje>? Pruebas_Dopaje { get; set; }
        public ICollection<Incidentes>? Incidentes { get; set; }
        public string estado { get; set; } = "Activo";
        public ICollection<Citas>? Citas { get; set; }
    }
}

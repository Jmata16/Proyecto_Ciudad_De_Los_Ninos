using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder los 50 caracteres.")]
        public string nombre_usuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder los 100 caracteres.")]
        public string apellidos { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha no válido.")]
        [Range(typeof(DateTime), "01/01/1900", "01/01/2100", ErrorMessage = "La fecha de nacimiento debe estar entre el 01/01/1900 y el 01/01/2100.")]
        public DateTime fecha_nacimiento { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cédula debe ser un número positivo.")]
        public int cedula { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        public string contraseña { get; set; }

        public int id_rol { get; set; }

        //[ForeignKey("id_rol")]
        //public Roles Rol { get; set; }  
        //public ICollection<Reportes_Expedientes> ReportesExpedientes { get; set; }
        //public ICollection<Reportes_Medicos> ReportesMedicos { get; set; }
        //public ICollection<Pruebas_Dopaje> PruebasDopaje { get; set; }
        //public ICollection<Incidentes> Incidentes { get; set; }
        public ICollection<Citas> ?Citas { get; set; }
        public ICollection<Incidentes>? Incidentes { get; set; }
        public ICollection<Reportes_Medicos>? ReportesMedicos { get; set; }
        public ICollection<Reportes_Expedientes>? Reportes_Expedientes { get; set; }
        public ICollection<Pruebas_Dopaje>? Pruebas_Dopaje { get; set; }
    }
}



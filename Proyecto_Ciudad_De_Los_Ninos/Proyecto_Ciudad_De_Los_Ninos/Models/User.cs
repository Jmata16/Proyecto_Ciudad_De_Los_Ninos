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

        public string nombre_usuario { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public int cedula { get; set; }
        public string contraseña { get; set; }
        public int id_rol { get; set; }

        //[ForeignKey("id_rol")]
        //public Roles Rol { get; set; }  
        //public ICollection<Reportes_Expedientes> ReportesExpedientes { get; set; }
        //public ICollection<Reportes_Medicos> ReportesMedicos { get; set; }
        //public ICollection<Pruebas_Dopaje> PruebasDopaje { get; set; }
        //public ICollection<Incidentes> Incidentes { get; set; }
        public ICollection<Citas> ?Citas { get; set; }
    }
}



using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public int IdRol { get; set; }

        public Rol Rol { get; set; }
        public ICollection<ReporteExpediente> ReportesExpedientes { get; set; }
        public ICollection<ReporteMedico> ReportesMedicos { get; set; }
        public ICollection<PruebaDopaje> PruebasDopaje { get; set; }
        public ICollection<Incidente> Incidentes { get; set; }
        public ICollection<Cita> Citas { get; set; }
    }


}


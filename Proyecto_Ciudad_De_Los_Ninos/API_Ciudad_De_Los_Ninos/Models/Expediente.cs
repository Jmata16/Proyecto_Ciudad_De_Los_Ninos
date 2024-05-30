using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Expediente
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int Id { get; set; }
        public int IdJoven { get; set; }
        public string NombreJoven { get; set; }
        public int Edad { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Direccion { get; set; }
        public string TelefonoContacto { get; set; }
        public string TutorLegal { get; set; }
        public string AntecedentesMedicos { get; set; }
        public string HistorialAcademico { get; set; }
        public string NotasAdicionales { get; set; }

        public Joven Joven { get; set; }
        public ICollection<ReporteExpediente> ReportesExpedientes { get; set; }
    }


}


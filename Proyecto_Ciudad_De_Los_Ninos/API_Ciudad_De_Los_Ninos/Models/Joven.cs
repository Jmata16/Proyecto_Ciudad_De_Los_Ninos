using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Joven
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }



       public string Nombre { get; set; }
       public int Edad { get; set; }
       public string Direccion { get; set; }
       public string TelefonoContacto { get; set; }

     public ICollection<Expediente> Expedientes { get; set; }
     public ICollection<ReporteMedico> ReportesMedicos { get; set; }
     public ICollection<PruebaDopaje> PruebasDopaje { get; set; }
     public ICollection<Incidente> Incidentes { get; set; }
     public ICollection<Cita> Citas { get; set; }
        




    }
}

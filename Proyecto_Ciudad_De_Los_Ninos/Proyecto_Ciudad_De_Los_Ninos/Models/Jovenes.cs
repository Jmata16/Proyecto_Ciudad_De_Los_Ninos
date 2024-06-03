using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Jovenes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public int cedula { get; set; }
        public string nombre { get; set; }
       public int edad { get; set; }
       public string direccion { get; set; }
       public string telefono_contacto { get; set; }

     public ICollection<Expedientes> Expedientes { get; set; }
     public ICollection<Reportes_Medicos> ReportesMedicos { get; set; }
     public ICollection<Pruebas_Dopaje> PruebasDopaje { get; set; }
     public ICollection<Incidentes> Incidentes { get; set; }
     public ICollection<Citas> Citas { get; set; }
        




    }
}

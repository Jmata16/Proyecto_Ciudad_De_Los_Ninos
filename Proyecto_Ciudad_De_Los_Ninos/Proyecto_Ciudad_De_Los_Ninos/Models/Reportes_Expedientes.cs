using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Reportes_Expedientes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int id_expediente { get; set; }
        public int id_usuario { get; set; }
        public string tipo { get; set; }
        public string contenido { get; set; }
        public DateTime fecha_creacion { get; set; }
        public string estado { get; set; } = "Activo";

        public Expedientes? Expedientes { get; set; }
        public User? Usuario { get; set; }
    }


}


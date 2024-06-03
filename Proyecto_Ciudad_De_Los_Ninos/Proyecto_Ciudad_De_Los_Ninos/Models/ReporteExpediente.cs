using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class ReporteExpediente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int IdExpediente { get; set; }
        public int IdUsuario { get; set; }
        public string Tipo { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Expediente Expediente { get; set; }
        public User Usuario { get; set; }
    }


}


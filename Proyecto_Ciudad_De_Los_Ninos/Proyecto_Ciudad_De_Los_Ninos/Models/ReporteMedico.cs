using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class ReporteMedico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdJoven { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Contenido { get; set; }

        public User Usuario { get; set; }
        public Joven Joven { get; set; }
    }
}


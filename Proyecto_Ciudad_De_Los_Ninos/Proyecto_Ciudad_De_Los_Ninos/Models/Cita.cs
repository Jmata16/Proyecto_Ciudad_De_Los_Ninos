using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Cita
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdJoven { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoUsuario { get; set; }
        public string Detalles { get; set; }

        public User Usuario { get; set; }
        public Joven Joven { get; set; }
    }

}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Incidentes
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int id_usuario { get; set; }
        public int id_joven { get; set; }
        public DateTime fecha_hora { get; set; }
        public string descripcion { get; set; }

        public User? Usuario { get; set; }
        public Jovenes? Joven { get; set; }
    }
}


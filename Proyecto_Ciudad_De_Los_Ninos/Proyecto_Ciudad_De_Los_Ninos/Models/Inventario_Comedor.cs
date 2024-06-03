using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Inventario_Comedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]



        public int Id { get; set; }
        public string nombre_alimento { get; set; }
        public int cantidad_disponible { get; set; }
        public DateTime fecha_ultima_reposicion { get; set; }
        public string proveedor { get; set; }
    }

}


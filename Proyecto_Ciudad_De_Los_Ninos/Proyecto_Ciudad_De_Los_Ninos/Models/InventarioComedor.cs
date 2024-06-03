using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class InventarioComedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]



        public int Id { get; set; }
        public string NombreAlimento { get; set; }
        public int CantidadDisponible { get; set; }
        public DateTime FechaUltimaReposicion { get; set; }
        public string Proveedor { get; set; }
    }

}


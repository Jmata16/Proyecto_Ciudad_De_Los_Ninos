using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class RifaEntry
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public int NumeroComprado { get; set; }
        public DateTime FechaCompra { get; set; }

        public int ?RifaId { get; set; }
        public Rifa ?Rifa { get; set; }
    }
}

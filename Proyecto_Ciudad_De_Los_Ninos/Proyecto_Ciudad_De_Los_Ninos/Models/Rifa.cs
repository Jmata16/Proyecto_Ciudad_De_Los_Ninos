using API_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Models
{
    public class Rifa
    {
        public int RifaId { get; set; }
        public DateTime FechaRifa { get; set; }
        public int? NumeroGanador { get; set; }
        public string Premio { get; set; }

        public ICollection<RifaEntry> ?RifaEntries { get; set; }
    }

}

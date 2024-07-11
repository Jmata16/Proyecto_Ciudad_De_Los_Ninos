using API_Ciudad_De_Los_Ninos.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Ciudad_De_Los_Ninos.Models
{
    public class Asistencia
    {
        public int ID { get; set; }

        public int id_usuario { get; set; }


        public string estado { get; set; }


        public DateTime fecha { get; set; } = DateTime.Today;  


        [DataType(DataType.DateTime)]
        public DateTime horaRegistro { get; set; } = DateTime.Now;  


        public DateTime? horaSalida { get; set; } 

        public string? justificacion { get; set; }

        // Relación con la tabla Users
        [ForeignKey("id_usuario")]
        public virtual User? User { get; set; }
    }
}

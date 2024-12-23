﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Expedientes
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int Id { get; set; }
        public int id_joven { get; set; }
        public string nombre_joven { get; set; }
        public DateTime fecha_ingreso { get; set; }
        public string tutor_legal { get; set; }
        public string antecedentes_medicos { get; set; }
        public string historial_academico { get; set; }
        public string notas_adicionales { get; set; }
        public string estado { get; set; } = "Activo";

        public Jovenes? Joven { get; set; }
        public ICollection<Reportes_Expedientes>? Reportes_Expedientes { get; set; }


    }


}


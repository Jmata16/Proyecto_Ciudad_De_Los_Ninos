using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Pruebas_Dopaje
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int id_usuario { get; set; }

        [ForeignKey("Joven")]
        public int id_joven { get; set; }

        [Required]
        public DateTime fecha { get; set; }

        [Required]
        [StringLength(255)]
        public string lugar { get; set; }

        [Required]
        [StringLength(8)]
        [RegularExpression("^(Positivo|Negativo)$", ErrorMessage = "El resultado debe ser 'Positivo' o 'Negativo'.")]
        public string resultado { get; set; }

        [StringLength(255)]
        public string observaciones { get; set; }

        public string estado { get; set; } = "Activo";

        public User? Usuario { get; set; }
        public Jovenes? Joven { get; set; }
    }
}

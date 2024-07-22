using API_Ciudad_De_Los_Ninos.Models;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class RegistroCompra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TicketeId { get; set; }

        [Required]
        [StringLength(255)]
        public string estado { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public int Inventario_HigieneId { get; set; }

        [ForeignKey("Inventario_HigieneId")]
        public Inventario_Higiene_Personal? Inventario_Higiene { get; set; }
    }
}

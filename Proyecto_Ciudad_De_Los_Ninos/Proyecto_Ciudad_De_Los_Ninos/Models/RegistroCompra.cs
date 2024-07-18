using API_Ciudad_De_Los_Ninos.Models;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models { 
public class RegistroCompra
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TicketeId { get; set; }

    [Required]
    public string estado { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
}
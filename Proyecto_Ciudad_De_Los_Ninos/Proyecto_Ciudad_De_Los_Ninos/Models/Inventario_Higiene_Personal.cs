using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Ciudad_De_Los_Ninos.Models
{
    public class Inventario_Higiene_Personal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre del producto no puede exceder los 50 caracteres.")]
        public string nombre_producto { get; set; }


        [Required(ErrorMessage = "La candidad disponible  es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad disponible debe ser un número positivo.")]
        public int cantidad_disponible { get; set; }


        [Required(ErrorMessage = "La fecha de la última reposición es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha no  es válido.")]
        [Range(typeof(DateTime), "01/01/1900", "01/01/2100", ErrorMessage = "La fecha de última reposición debe estar entre el 01/01/2000 y el 01/01/2100.")]
        public DateTime fecha_ultima_reposicion { get; set; }


        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        [StringLength(60, ErrorMessage = "El nombre del proveedor no puede exceder los 60 caracteres.")]
        public string proveedor { get; set; }
    }
}


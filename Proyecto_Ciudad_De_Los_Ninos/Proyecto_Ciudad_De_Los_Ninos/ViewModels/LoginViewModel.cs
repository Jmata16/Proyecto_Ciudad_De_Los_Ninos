using System.ComponentModel.DataAnnotations;

namespace Proyecto_Ciudad_De_Los_Ninos.Models
{
    public class LoginViewModel
         
    {


        public int ID { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string nombre_usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string contraseña { get; set; }

        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }
    }
}

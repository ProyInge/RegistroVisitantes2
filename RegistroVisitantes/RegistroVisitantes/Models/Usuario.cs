using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegistroVisitantes.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El primer nombre es requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El segundo nombre es requerido.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email es requerido.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Por favor ingrese un corre electrónico válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
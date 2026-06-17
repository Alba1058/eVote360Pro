using System.ComponentModel.DataAnnotations;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Usuarios
{
    public class SaveUsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string NombreUsuario { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluir una letra y un número.")]
        public string Contrasena { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare(nameof(Contrasena), ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        public string ConfirmarContrasena { get; set; } = string.Empty;

        public bool CambiarContrasena { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        public RolUsuario Rol { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

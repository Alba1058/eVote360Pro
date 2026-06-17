using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Ciudadania
{
    public class SaveCiudadanoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de documento es requerido")]
        public string NumeroDocumento { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public bool BloquearDocumento { get; set; }
    }
}

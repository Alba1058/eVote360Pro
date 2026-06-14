using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Ciudadania
{
    public class SaveCiudadanoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [DataType(DataType.Text)]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [DataType(DataType.Text)]
        public required string Apellido { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [DataType(DataType.EmailAddress)]
        public required string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El número de documento es requerido")]
        [DataType(DataType.Text)]
        public required string NumeroDocumento { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

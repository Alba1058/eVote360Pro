using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Elector
{
    public class VerificarCodigoViewModel
    {
        public string NumeroDocumento { get; set; } = string.Empty;
        public int CiudadanoId { get; set; }

        [Required(ErrorMessage = "Debe ingresar el código de verificación enviado a su correo electrónico.")]
        public string CodigoVerificacion { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace eVote360Pro.Core.Application.ViewModels.Elector
{
    public class ValidarIdentidadViewModel
    {
        public string NumeroDocumento { get; set; } = string.Empty;
        public int CiudadanoId { get; set; }

        [Required(ErrorMessage = "La imagen de la cédula es requerida")]
        public IFormFile? ImagenCedula { get; set; }
    }
}

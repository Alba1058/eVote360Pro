using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Elector
{
    public class VotarViewModel
    {
        [Required(ErrorMessage = "El número de documento de identidad es requerido")]
        public string NumeroDocumento { get; set; } = string.Empty;
    }
}

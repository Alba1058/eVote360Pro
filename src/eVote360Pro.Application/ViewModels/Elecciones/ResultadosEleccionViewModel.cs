using eVote360Pro.Core.Application.ViewModels.Elecciones;

namespace eVote360Pro.Core.Application.ViewModels.Elecciones
{
    public class ResultadosEleccionViewModel
    {
        public int EleccionId { get; set; }
        public string EleccionNombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public List<ResultadoPuestoViewModel> Puestos { get; set; } = [];
    }

    public class ResultadoPuestoViewModel
    {
        public string PuestoNombre { get; set; } = string.Empty;
        public bool ExisteEmpate { get; set; }
        public List<ResultadoOpcionViewModel> Opciones { get; set; } = [];
    }

    public class ResultadoOpcionViewModel
    {
        public string CandidatoNombre { get; set; } = string.Empty;
        public string PartidoNombre { get; set; } = string.Empty;
        public int CantidadVotos { get; set; }
        public decimal Porcentaje { get; set; }
        public bool EsGanador { get; set; }
    }
}

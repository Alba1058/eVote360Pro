namespace eVote360Pro.Core.Application.Dtos.Elecciones
{
    public class ResultadoOpcionDto
    {
        public string PuestoNombre { get; set; } = string.Empty;
        public string CandidatoNombre { get; set; } = string.Empty;
        public string PartidoNombre { get; set; } = string.Empty;
        public int CantidadVotos { get; set; }
        public decimal Porcentaje { get; set; }
        public bool EsGanador { get; set; }
        public bool HayEmpate { get; set; }
    }

    public class ResultadoPuestoDto
    {
        public int PuestoElectivoId { get; set; }
        public string PuestoNombre { get; set; } = string.Empty;
        public bool ExisteEmpate { get; set; }
        public List<ResultadoOpcionDto> Opciones { get; set; } = [];
    }
}

using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.ViewModels.Elector;

namespace eVote360Pro.Core.Application.Mappings.Elector
{
    public class VotacionMappingProfile : Profile
    {
        public VotacionMappingProfile()
        {
            CreateMap<PuestoVotacionDto, PuestoElectivoVotacionViewModel>();
            CreateMap<CandidatoVotacionDto, CandidatoVotacionViewModel>();
        }
    }
}

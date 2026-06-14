using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.ViewModels.Partidos;

namespace eVote360Pro.Core.Application.Mappings.DtosAndViewModels.Partidos
{
    public class AsignacionCandidatoPuestoDtoMappingProfile : Profile
    {
        public AsignacionCandidatoPuestoDtoMappingProfile()
        {
            CreateMap<AsignacionCandidatoPuestoDto, AsignacionCandidatoPuestoViewModel>()
                .ReverseMap();

            CreateMap<SaveAsignacionCandidatoPuestoDto, SaveAsignacionCandidatoPuestoViewModel>()
                .ReverseMap();
        }
    }
}

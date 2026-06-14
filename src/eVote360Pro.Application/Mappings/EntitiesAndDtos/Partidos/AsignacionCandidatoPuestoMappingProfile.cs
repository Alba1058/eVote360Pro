using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Domain.Entities.Partidos;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Partidos
{
    public class AsignacionCandidatoPuestoMappingProfile : Profile
    {
        public AsignacionCandidatoPuestoMappingProfile()
        {
            CreateMap<AsignacionCandidatoPuesto, AsignacionCandidatoPuestoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Candidato, opt => opt.Ignore())
                .ForMember(dest => dest.PuestoElectivo, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore());

            CreateMap<AsignacionCandidatoPuesto, SaveAsignacionCandidatoPuestoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Candidato, opt => opt.Ignore())
                .ForMember(dest => dest.PuestoElectivo, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore());
        }
    }
}

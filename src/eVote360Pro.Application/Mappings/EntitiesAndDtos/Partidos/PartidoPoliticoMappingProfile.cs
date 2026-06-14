using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Domain.Entities.Partidos;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Partidos
{
    public class PartidoPoliticoMappingProfile : Profile
    {
        public PartidoPoliticoMappingProfile()
        {
            CreateMap<PartidoPolitico, PartidoPoliticoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Candidatos, opt => opt.Ignore())
                .ForMember(dest => dest.AsignacionesCandidatos, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudesEnviadas, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudesRecibidas, opt => opt.Ignore())
                .ForMember(dest => dest.AlianzasComoPartido1, opt => opt.Ignore())
                .ForMember(dest => dest.AlianzasComoPartido2, opt => opt.Ignore())
                .ForMember(dest => dest.Usuarios, opt => opt.Ignore());

            CreateMap<PartidoPolitico, SavePartidoPoliticoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Candidatos, opt => opt.Ignore())
                .ForMember(dest => dest.AsignacionesCandidatos, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudesEnviadas, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudesRecibidas, opt => opt.Ignore())
                .ForMember(dest => dest.AlianzasComoPartido1, opt => opt.Ignore())
                .ForMember(dest => dest.AlianzasComoPartido2, opt => opt.Ignore())
                .ForMember(dest => dest.Usuarios, opt => opt.Ignore());
        }
    }
}

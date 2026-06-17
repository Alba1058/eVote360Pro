using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Alianzas;
using eVote360Pro.Core.Domain.Entities.Alianzas;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Alianzas
{
    public class SolicitudAlianzaMappingProfile : Profile
    {
        public SolicitudAlianzaMappingProfile()
        {
            CreateMap<SolicitudAlianza, SolicitudAlianzaDto>()
                .ForMember(dest => dest.NombrePartidoSolicitante, opt => opt.MapFrom(src => src.PartidoSolicitante != null ? src.PartidoSolicitante.Nombre : string.Empty))
                .ForMember(dest => dest.SiglasPartidoSolicitante, opt => opt.MapFrom(src => src.PartidoSolicitante != null ? src.PartidoSolicitante.Siglas : string.Empty))
                .ForMember(dest => dest.NombrePartidoReceptor, opt => opt.MapFrom(src => src.PartidoReceptor != null ? src.PartidoReceptor.Nombre : string.Empty))
                .ForMember(dest => dest.SiglasPartidoReceptor, opt => opt.MapFrom(src => src.PartidoReceptor != null ? src.PartidoReceptor.Siglas : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.PartidoSolicitante, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoReceptor, opt => opt.Ignore());

            CreateMap<SolicitudAlianza, SaveSolicitudAlianzaDto>()
                .ReverseMap()
                .ForMember(dest => dest.FechaSolicitud, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoSolicitante, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoReceptor, opt => opt.Ignore());
        }
    }
}

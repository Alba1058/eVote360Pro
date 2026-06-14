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

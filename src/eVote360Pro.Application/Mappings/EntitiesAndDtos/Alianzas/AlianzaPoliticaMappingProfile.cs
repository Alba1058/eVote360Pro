using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Alianzas;
using eVote360Pro.Core.Domain.Entities.Alianzas;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Alianzas
{
    public class AlianzaPoliticaMappingProfile : Profile
    {
        public AlianzaPoliticaMappingProfile()
        {
            CreateMap<AlianzaPolitica, AlianzaPoliticaDto>()
                .ForMember(dest => dest.NombrePartido1, opt => opt.MapFrom(src => src.Partido1 != null ? src.Partido1.Nombre : string.Empty))
                .ForMember(dest => dest.SiglasPartido1, opt => opt.MapFrom(src => src.Partido1 != null ? src.Partido1.Siglas : string.Empty))
                .ForMember(dest => dest.NombrePartido2, opt => opt.MapFrom(src => src.Partido2 != null ? src.Partido2.Nombre : string.Empty))
                .ForMember(dest => dest.SiglasPartido2, opt => opt.MapFrom(src => src.Partido2 != null ? src.Partido2.Siglas : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Partido1, opt => opt.Ignore())
                .ForMember(dest => dest.Partido2, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudAlianza, opt => opt.Ignore());

            CreateMap<AlianzaPolitica, SaveAlianzaPoliticaDto>()
                .ReverseMap()
                .ForMember(dest => dest.FechaAceptacion, opt => opt.Ignore())
                .ForMember(dest => dest.Partido1, opt => opt.Ignore())
                .ForMember(dest => dest.Partido2, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudAlianza, opt => opt.Ignore());
        }
    }
}

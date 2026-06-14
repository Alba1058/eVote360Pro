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

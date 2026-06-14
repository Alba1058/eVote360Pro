using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Ciudadania;
using eVote360Pro.Core.Domain.Entities.Ciudadania;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Ciudadania
{
    public class CiudadanoMappingProfile : Profile
    {
        public CiudadanoMappingProfile()
        {
            CreateMap<Ciudadano, CiudadanoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Votos, opt => opt.Ignore())
                .ForMember(dest => dest.CodigosVerificacion, opt => opt.Ignore());

            CreateMap<Ciudadano, SaveCiudadanoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Votos, opt => opt.Ignore())
                .ForMember(dest => dest.CodigosVerificacion, opt => opt.Ignore());
        }
    }
}

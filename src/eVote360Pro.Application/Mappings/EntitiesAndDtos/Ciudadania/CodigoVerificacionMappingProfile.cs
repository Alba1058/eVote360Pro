using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Ciudadania;
using eVote360Pro.Core.Domain.Entities.Ciudadania;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Ciudadania
{
    public class CodigoVerificacionMappingProfile : Profile
    {
        public CodigoVerificacionMappingProfile()
        {
            CreateMap<CodigoVerificacion, CodigoVerificacionDto>()
                .ReverseMap()
                .ForMember(dest => dest.Ciudadano, opt => opt.Ignore())
                .ForMember(dest => dest.Eleccion, opt => opt.Ignore());
        }
    }
}

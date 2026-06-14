using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Domain.Entities.Elecciones;

namespace eVote360Pro.Core.Application.Mappings.EntitiesAndDtos.Elecciones
{
    public class EleccionMappingProfile : Profile
    {
        public EleccionMappingProfile()
        {
            CreateMap<Eleccion, EleccionDto>()
                .ReverseMap()
                .ForMember(dest => dest.EleccionesPuestos, opt => opt.Ignore())
                .ForMember(dest => dest.Votos, opt => opt.Ignore());

            CreateMap<Eleccion, SaveEleccionDto>()
                .ReverseMap()
                .ForMember(dest => dest.EleccionesPuestos, opt => opt.Ignore())
                .ForMember(dest => dest.Votos, opt => opt.Ignore());
        }
    }
}
